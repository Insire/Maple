using Maple.Domain;
using Maple.Properties;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Observables;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Maple
{
    public sealed class ShellViewModel : BusinessViewModelBase
    {
        private readonly ILogger<ShellViewModel> _log;
        private readonly Func<ApplicationDbContext> _dbcontextFactory;
        private readonly PlaylistFactory _playlistFactory;
        private readonly MediaPlayerFactory _mediaPlayerFactory;
        private readonly Func<IAudioDeviceProvider> _audioDeviceFactory;

        public MetaDataViewModel MetaDataViewModel { get; }

        public NavigationViewModel NavigationViewModel { get; }

        public LocalizationsViewModel Localizations { get; }

        public Playlists Playlists { get; }
        public MediaPlayers MediaPlayers { get; }
        public AudioDevices AudioDevices { get; }
        public AudioDeviceTypes AudioDeviceTypes { get; }
        public OptionsViewModel OptionsViewModel { get; }

        public ShellViewModel(IScarletCommandBuilder commandBuilder,
                                ILoggerFactory loggerFactory,
                                LocalizationsViewModel localizationsViewModel,
                                NavigationViewModel navigationViewModel,
                                MetaDataViewModel metaDataViewModel,
                                Playlists playlists,
                                MediaPlayers mediaPlayers,
                                AudioDevices audioDevices,
                                AudioDeviceTypes audioDeviceTypes,
                                OptionsViewModel optionsViewModel,
                                Func<ApplicationDbContext> dbcontextFactory,
                                PlaylistFactory playlistFactory,
                                MediaPlayerFactory mediaPlayerFactory,
                                Func<IAudioDeviceProvider> audioDeviceFactory)
            : base(commandBuilder)
        {
            _log = loggerFactory.CreateLogger<ShellViewModel>();

            Localizations = localizationsViewModel ?? throw new ArgumentNullException(nameof(localizationsViewModel));
            NavigationViewModel = navigationViewModel ?? throw new ArgumentNullException(nameof(navigationViewModel));
            MetaDataViewModel = metaDataViewModel ?? throw new ArgumentNullException(nameof(metaDataViewModel));
            Playlists = playlists ?? throw new ArgumentNullException(nameof(playlists));
            MediaPlayers = mediaPlayers ?? throw new ArgumentNullException(nameof(mediaPlayers));
            AudioDevices = audioDevices ?? throw new ArgumentNullException(nameof(audioDevices));
            AudioDeviceTypes = audioDeviceTypes ?? throw new ArgumentNullException(nameof(audioDeviceTypes));
            OptionsViewModel = optionsViewModel ?? throw new ArgumentNullException(nameof(optionsViewModel));

            _dbcontextFactory = dbcontextFactory ?? throw new ArgumentNullException(nameof(dbcontextFactory));
            _playlistFactory = playlistFactory ?? throw new ArgumentNullException(nameof(playlistFactory));
            _mediaPlayerFactory = mediaPlayerFactory ?? throw new ArgumentNullException(nameof(mediaPlayerFactory));
            _audioDeviceFactory = audioDeviceFactory ?? throw new ArgumentNullException(nameof(audioDeviceFactory));
        }

        protected override async Task RefreshInternal(CancellationToken token)
        {
            try
            {
                using (var context = _dbcontextFactory())
                {
                    context.Database.Migrate();

                    await Playlists.Clear();
                    await AudioDeviceTypes.Clear();
                    await MediaPlayers.Clear();

                    // TODO fetch all settings

                    await GetAudioDeviceTypes(context, token);
                    await GetAudioDevices(context, token);
                    await GetPlaylists(context, token);
                    await GetMediaPlayers(context, token);

                    // TODO fetch and set selections

                    await context.SaveChangesAsync(token);
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex, Resources.AppStartLoadDataFailed);
                throw;
            }
        }

        private async Task GetMediaPlayers(ApplicationDbContext context, CancellationToken token)
        {
            var players = await context.MediaPlayers.ToListAsync(token);

            foreach (var player in players)
            {
                var viewmodel = _mediaPlayerFactory.Create(player);

                viewmodel.Playlist = Playlists.GetById(viewmodel.PlaylistId);
                viewmodel.AudioDevice = AudioDevices.GetById(viewmodel.AudioDeviceId);

                await MediaPlayers.Add(viewmodel);
            }
        }

        private async Task GetPlaylists(ApplicationDbContext context, CancellationToken token)
        {
            var playlists = await context.Playlists
                .Include(p => p.MediaItems)
                .ToListAsync(token);

            foreach (var playlist in playlists)
            {
                var viewmodel = _playlistFactory.Create(playlist);

                await Playlists.Add(viewmodel);
            }
        }

        private async Task GetAudioDeviceTypes(ApplicationDbContext context, CancellationToken token)
        {
            var models = await context.AudioDeviceTypes.OrderBy(p => p.Sequence).ThenBy(p => p.Name).ToListAsync(token);

            await AudioDeviceTypes.AddRange(models.Select(p => new AudioDeviceType(p)));
        }

        private async Task GetAudioDevices(ApplicationDbContext context, CancellationToken token)
        {
            var audioDeviceFactory = _audioDeviceFactory();
            await AudioDevices.Clear();

            var models = await context.AudioDevices.OrderBy(p => p.Sequence).ThenBy(p => p.AudioDeviceTypeId).ThenBy(p => p.Name).ToListAsync(token);
            var devices = await audioDeviceFactory.Get(token);
            var deviceLookup = devices.ToDictionary(p => p.GetKey());
            var modelLookup = models.ToDictionary(p => p.OsId.GetHashCode() ^ p.AudioDeviceTypeId);

            // add entries that are in the db, but not present in the system anymore
            foreach (var model in models)
            {
                var key = model.GetKey();
                if (deviceLookup.ContainsKey(key))
                {
                    // update the device
                    var device = deviceLookup[key];
                    device.UpdateFromModel(model, AudioDeviceTypes.GetById(device.Id));

                    await AudioDevices.Add(device);
                }
                else
                {
                    // delete the device
                    context.AudioDevices.Remove(model);
                }
            }

            // create missing models
            foreach (var device in devices)
            {
                var key = device.GetKey();
                if (modelLookup.ContainsKey(key))
                {
                    // update the device
                    var model = modelLookup[key];
                    device.UpdateFromModel(model, AudioDeviceTypes.GetById(device.Id));

                    await AudioDevices.Add(device);
                }
                else
                {
                    var model = device.GetModel();
                    await AudioDevices.Add(device);

                    // insert the new device
                    context.AudioDevices.Add(model);
                    device.UpdateFromModel(model, AudioDeviceTypes.GetById(device.Id));
                }
            }
        }

        protected override async Task UnloadInternal(CancellationToken token)
        {
            using (var context = _dbcontextFactory())
            {
                await SaveAudioDeviceTypes(context, token);
                await SaveAudioDevices(context, token);
                await SavePlaylists(context, token);
                await SaveMediaPlayers(context, token);

                await context.SaveChangesAsync(token);
            }
        }

        private async Task SaveAudioDeviceTypes(ApplicationDbContext context, CancellationToken token)
        {
            // no insert required - only updates and deletes here
            var viewModels = AudioDeviceTypes.Items;
            var query = context.AudioDeviceTypes.AsTracking().AsQueryable();
            foreach (var viewModel in viewModels)
            {
                query = query.Where(p => viewModel.Id == p.Id);
            }

            var models = await query.ToArrayAsync(token);
            var lookup = models.ToDictionary(p => p.Id);
            var now = DateTime.Now;

            foreach (var viewModel in viewModels)
            {
                var model = lookup[viewModel.Id];
                if (viewModel.IsDeleted)
                {
                    context.AudioDeviceTypes.Remove(model);
                    continue;
                }

                model.Name = viewModel.Name;
                model.Sequence = viewModel.Sequence;

                model.DeviceType = viewModel.DeviceType;

                model.UpdatedOn = now;
                model.UpdatedBy = Environment.UserName;
            }
        }

        private async Task SaveAudioDevices(ApplicationDbContext context, CancellationToken token)
        {
            var viewModels = AudioDevices.Items;
            var query = context.AudioDevices.AsTracking().AsQueryable();
            foreach (var viewModel in viewModels)
            {
                query = query.Where(p => viewModel.Id == p.Id);
            }

            var models = await query.ToArrayAsync(token);
            var lookup = models.ToDictionary(p => p.Id);
            var now = DateTime.Now;

            foreach (var viewModel in viewModels)
            {
                if (viewModel.IsNew())
                {
                    var model = viewModel.GetModel();

                    context.AudioDevices.Add(model);
                    continue;
                }
                else
                {
                    var model = lookup[viewModel.Id];
                    if (viewModel.IsDeleted)
                    {
                        context.AudioDevices.Remove(model);
                        continue;
                    }

                    model.Name = viewModel.Name;
                    model.Sequence = viewModel.Sequence;

                    model.AudioDeviceTypeId = viewModel.AudioDeviceTypeId;

                    model.UpdatedOn = now;
                    model.UpdatedBy = Environment.UserName;
                }
            }
        }

        private async Task SavePlaylists(ApplicationDbContext context, CancellationToken token)
        {
            var viewModels = Playlists.Items;
            var query = context.Playlists.AsTracking().AsQueryable();
            foreach (var viewModel in viewModels)
            {
                query = query.Where(p => viewModel.Id == p.Id);
            }

            var models = await query.ToArrayAsync(token);
            var lookup = models.ToDictionary(p => p.Id);
            var now = DateTime.Now;

            foreach (var viewModel in viewModels)
            {
                if (viewModel.IsNew())
                {
                    var model = viewModel.GetModel();

                    context.Playlists.Add(model);
                    await SaveMediaItems(viewModel);
                    continue;
                }
                else
                {
                    var model = lookup[viewModel.Id];
                    if (viewModel.IsDeleted)
                    {
                        context.Playlists.Remove(model);
                        continue;
                    }

                    model.Name = viewModel.Name;
                    model.Sequence = viewModel.Sequence;

                    model.Thumbnail = viewModel.Thumbnail;
                    model.IsShuffeling = viewModel.IsShuffeling;
                    model.PrivacyStatus = viewModel.PrivacyStatus;
                    model.RepeatMode = viewModel.RepeatMode;

                    model.UpdatedOn = now;
                    model.UpdatedBy = Environment.UserName;

                    await SaveMediaItems(viewModel);
                }
            }

            async Task SaveMediaItems(Playlist playlist)
            {
                var query = context.MediaItems.AsTracking().AsQueryable();
                foreach (var viewModel in playlist.Items)
                {
                    query = query.Where(p => viewModel.Id == p.Id);
                }

                var models = await query.ToArrayAsync(token);
                var lookup = models.ToDictionary(p => p.Id);

                foreach (var viewModel in playlist.Items)
                {
                    if (viewModel.IsNew())
                    {
                        var model = viewModel.GetModel();

                        context.MediaItems.Add(model);
                        continue;
                    }
                    else
                    {
                        var model = lookup[viewModel.Id];
                        if (viewModel.IsDeleted)
                        {
                            context.MediaItems.Remove(model);
                            continue;
                        }

                        model.Name = viewModel.Name;
                        model.Sequence = viewModel.Sequence;

                        model.Location = viewModel.Location;
                        model.Duration = viewModel.Duration;
                        model.PrivacyStatus = viewModel.PrivacyStatus;
                        model.MediaItemType = viewModel.MediaItemType;

                        model.UpdatedOn = now;
                        model.UpdatedBy = Environment.UserName;
                    }
                }
            }
        }

        private async Task SaveMediaPlayers(ApplicationDbContext context, CancellationToken token)
        {
            var viewModels = MediaPlayers.Items;
            var query = context.MediaPlayers.AsTracking().AsQueryable();
            foreach (var viewModel in viewModels)
            {
                query = query.Where(p => viewModel.Id == p.Id);
            }

            var models = await query.ToArrayAsync(token);
            var lookup = models.ToDictionary(p => p.Id);
            var now = DateTime.Now;

            foreach (var viewModel in viewModels)
            {
                if (viewModel.IsNew())
                {
                    var model = viewModel.GetModel();

                    context.MediaPlayers.Add(model);
                    continue;
                }
                else
                {
                    var model = lookup[viewModel.Id];
                    if (viewModel.IsDeleted)
                    {
                        context.MediaPlayers.Remove(model);
                        continue;
                    }

                    model.Name = viewModel.Name;
                    model.Sequence = viewModel.Sequence;

                    model.AudioDeviceId = viewModel.AudioDeviceId;
                    model.IsPrimary = viewModel.IsPrimary;
                    model.PlaylistId = viewModel.PlaylistId;

                    model.UpdatedOn = now;
                    model.UpdatedBy = Environment.UserName;
                }
            }
        }
    }
}
