using System;
using System.Linq;
using System.Threading.Tasks;

using Maple.Core;
using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple
{
    public sealed class MediaPlayers : BusinessListViewModel<MediaPlayer, MediaPlayerModel>, IMediaPlayersViewModel
    {
        private readonly Func<IUnitOfWork> _repositoryFactory;
        private readonly IMediaPlayerMapper _mediaPlayerMapper;
        private readonly ILoggingNotifcationService _notificationService;

        public IPlaylistsViewModel Playlists { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaPlayers"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="playerFactory">The player factory.</param>
        /// <param name="repositoryFactory">The repo.</param>
        /// <param name="devices">The devices.</param>
        /// <param name="dialog">The dialog.</param>
        public MediaPlayers(ViewModelServiceContainer container,
                            IMediaPlayerMapper mediaPlayerMapper,
                            Func<IUnitOfWork> repositoryFactory,
                            IPlaylistsViewModel playlists)
            : base(container)
        {
            _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory), $"{nameof(repositoryFactory)} {Resources.IsRequired}");
            _mediaPlayerMapper = mediaPlayerMapper ?? throw new ArgumentNullException(nameof(mediaPlayerMapper), $"{nameof(mediaPlayerMapper)} {Resources.IsRequired}");

            _notificationService = container.NotificationService;

            Playlists = playlists ?? throw new ArgumentNullException(nameof(playlists), $"{nameof(playlists)} {Resources.IsRequired}");
        }

        public Task Add()
        {
            var sequence = _sequenceProvider.Get(Items.Cast<ISequence>().ToList());
            Add(_mediaPlayerMapper.GetNewMediaPlayer(sequence));

            return Task.CompletedTask;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                foreach (var player in Items)
                    player?.Dispose();

                // Free any other managed objects here.
            }

            // Free any unmanaged objects here.
            Disposed = true;
        }

        public override async Task Load()
        {
            _notificationService.Info($"{_translationService.Translate(nameof(Resources.Loading))} {_translationService.Translate(nameof(Resources.MediaPlayers))}");
            Clear();

            using (var context = _repositoryFactory())
            {
                var main = await context.MediaPlayerRepository.GetMainMediaPlayerAsync().ConfigureAwait(true);
                if (!(main is null))
                {
                    var viewModel = _mediaPlayerMapper.Get(main);
                    Add(viewModel);
                    SelectedItem = viewModel;
                }
                else
                {
                    main = new MediaPlayerModel()
                    {
                        IsPrimary = true,
                        Name = "Primary",
                        Sequence = _sequenceProvider.Get(Items.Cast<ISequence>().ToList()),
                    };

                    if (!Playlists.IsLoaded)
                    {
                        await Playlists.Load();
                    }

                    if (Playlists.Count > 0)
                    {
                        var viewModel = _mediaPlayerMapper.GetMain(main, Playlists[0]);
                        Add(viewModel);
                        SelectedItem = viewModel;
                    }
                    else
                    {
                        await Playlists.Add();
                        var viewModel = _mediaPlayerMapper.GetMain(main, Playlists[0]);
                        Add(viewModel);
                        SelectedItem = viewModel;
                    }
                }

                var others = await context.MediaPlayerRepository.GetOptionalMediaPlayersAsync().ConfigureAwait(true);
                if (others.Count > 0)
                {
                    AddRange(_mediaPlayerMapper.GetMany(others));
                }
            }

            OnLoaded();
        }

        public override Task Save()
        {
            return Task.CompletedTask;
        }
    }
}
