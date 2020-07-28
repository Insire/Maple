using System;
using System.Threading;
using System.Threading.Tasks;
using Maple.Properties;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    internal sealed class ShellViewModel : BusinessViewModelBase
    {
        private readonly ILogger<ShellViewModel> _log;
        private readonly Func<ApplicationDbContext> _dbcontextFactory;
        private readonly PlaylistFactory _playlistFactory;
        private readonly MediaPlayerFactory _mediaPlayerFactory;
        private readonly Func<AudioDeviceFactory> _audioDeviceFactory;

        public MetaDataViewModel MetaDataViewModel { get; }

        public NavigationViewModel NavigationViewModel { get; }

        public LocalizationsViewModel Localizations { get; }

        public Playlists Playlists { get; }
        public MediaPlayers MediaPlayers { get; }
        public AudioDevices AudioDevices { get; }
        public OptionsViewModel OptionsViewModel { get; }

        public ShellViewModel(IScarletCommandBuilder commandBuilder,
                                ILoggerFactory loggerFactory,
                                LocalizationsViewModel localizationsViewModel,
                                NavigationViewModel navigationViewModel,
                                MetaDataViewModel metaDataViewModel,
                                Playlists playlists,
                                MediaPlayers mediaPlayers,
                                AudioDevices audioDevices,
                                OptionsViewModel optionsViewModel,
                                Func<ApplicationDbContext> dbcontextFactory,
                                PlaylistFactory playlistFactory,
                                MediaPlayerFactory mediaPlayerFactory,
                                Func<AudioDeviceFactory> audioDeviceFactory)
            : base(commandBuilder)
        {
            _log = loggerFactory.CreateLogger<ShellViewModel>();
            Localizations = localizationsViewModel ?? throw new ArgumentNullException(nameof(localizationsViewModel));
            NavigationViewModel = navigationViewModel ?? throw new ArgumentNullException(nameof(navigationViewModel));
            MetaDataViewModel = metaDataViewModel ?? throw new ArgumentNullException(nameof(metaDataViewModel));
            Playlists = playlists ?? throw new ArgumentNullException(nameof(playlists));
            MediaPlayers = mediaPlayers ?? throw new ArgumentNullException(nameof(mediaPlayers));
            AudioDevices = audioDevices ?? throw new ArgumentNullException(nameof(audioDevices));
            OptionsViewModel = optionsViewModel ?? throw new ArgumentNullException(nameof(optionsViewModel));

            _dbcontextFactory = dbcontextFactory ?? throw new ArgumentNullException(nameof(dbcontextFactory));
            _playlistFactory = playlistFactory ?? throw new ArgumentNullException(nameof(playlistFactory));
            _mediaPlayerFactory = mediaPlayerFactory ?? throw new ArgumentNullException(nameof(mediaPlayerFactory));
            _audioDeviceFactory = audioDeviceFactory ?? throw new ArgumentNullException(nameof(audioDeviceFactory));
        }

        protected override Task UnloadInternal(CancellationToken token)
        {
            // serialize all media players first, thgen serialize playlists

            return Task.CompletedTask;
        }

        protected override async Task RefreshInternal(CancellationToken token)
        {
            try
            {
                using (var context = _dbcontextFactory())
                {
                    context.Database.EnsureCreated();

                    // fetch all settings

                    // TODO

                    // fetch all playlists and their mediaitems
                    var playlists = await context.Playlists.ToListAsync(token);
                    await Playlists.Clear();

                    foreach (var playlist in playlists)
                    {
                        var viewmodel = _playlistFactory.Create(playlist);

                        await Playlists.Add(viewmodel);
                    }

                    // fetch all audio devices
                    var devices = await context.AudioDevices.ToListAsync(token);
                    await AudioDevices.Clear();

                    var audioDeviceFactory = _audioDeviceFactory();
                    foreach (var device in devices)
                    {
                        var viewmodel = await audioDeviceFactory.Create(device, token);

                        await AudioDevices.Add(viewmodel);
                    }

                    // fetch all media players
                    var players = await context.Mediaplayers.ToListAsync(token);
                    await MediaPlayers.Clear();

                    foreach (var player in players)
                    {
                        var viewmodel = _mediaPlayerFactory.Create(player);

                        await MediaPlayers.Add(viewmodel);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex, Resources.AppStartLoadDataFailed);
                throw;
            }

            // current/ last mediaplayer
            // current /last culture

            // deserialize all media players first, thgen deserialize playlists
        }
    }
}
