using Maple.Core;
using Maple.Localization.Properties;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Maple
{
    public class MediaPlayers : BaseDataListViewModel<MediaPlayer, Data.MediaPlayer>, IMediaPlayersViewModel
    {
        private readonly Func<IMediaPlayer> _playerFactory;
        private readonly AudioDevices _devices;
        private readonly DialogViewModel _dialog;
        private readonly Func<IMediaRepository> _repositoryFactory;
        private readonly IMediaPlayerMapper _mediaPlayerMapper;
        private readonly ILoggingNotifcationService _notificationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaPlayers"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="playerFactory">The player factory.</param>
        /// <param name="repo">The repo.</param>
        /// <param name="devices">The devices.</param>
        /// <param name="dialog">The dialog.</param>
        public MediaPlayers(ViewModelServiceContainer container, IMediaPlayerMapper mediaPlayerMapper, Func<IMediaPlayer> playerFactory, Func<IMediaRepository> repo, AudioDevices devices, DialogViewModel dialog)
            : base(container)
        {
            _playerFactory = playerFactory ?? throw new ArgumentNullException(nameof(playerFactory));
            _devices = devices ?? throw new ArgumentNullException(nameof(devices));
            _dialog = dialog ?? throw new ArgumentNullException(nameof(dialog));
            _repositoryFactory = repo ?? throw new ArgumentNullException(nameof(repo));
            _mediaPlayerMapper = mediaPlayerMapper ?? throw new ArgumentNullException(nameof(mediaPlayerMapper));

            _notificationService = container.NotificationService;
        }

        private void SaveInternal()
        {
            _log.Info($"{_translationService.Translate(nameof(Resources.Saving))} {_translationService.Translate(nameof(Resources.MediaPlayers))}");
            using (var context = _repositoryFactory())
            {
                context.Save(this);
            }
        }

        public void Add()
        {
            var sequence = _sequenceProvider.Get(Items.Select(p => (ISequence)p).ToList());
            Add(_mediaPlayerMapper.GetNewMediaPlayer(sequence));
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

                base.Dispose(disposing);
                // Free any other managed objects here.
            }

            // Free any unmanaged objects here.
            Disposed = true;
        }

        public override void Save()
        {
             SaveInternal();
        }

        public override async Task LoadAsync()
        {
            _notificationService.Info($"{_translationService.Translate(nameof(Resources.Loading))} {_translationService.Translate(nameof(Resources.MediaPlayers))}");
            Items.Clear();

            using (var context = _repositoryFactory())
            {
                var main = await context.GetMainMediaPlayerAsync();

                Items.Add(main);
                SelectedItem = main;

                var others = await context.GetAllOptionalMediaPlayersAsync();
                Items.AddRange(others);
            }

            OnLoaded();
        }
    }
}
