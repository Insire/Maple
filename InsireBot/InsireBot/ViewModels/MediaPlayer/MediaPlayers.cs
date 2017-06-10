using Maple.Core;
using Maple.Localization.Properties;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Maple
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Maple.Core.BaseDataListViewModel{Maple.MediaPlayer, Maple.Data.MediaPlayer}" />
    /// <seealso cref="System.IDisposable" />
    /// <seealso cref="Maple.Core.ILoadableViewModel" />
    /// <seealso cref="Maple.Core.ISaveableViewModel" />
    public class MediaPlayers : BaseDataListViewModel<MediaPlayer, Data.MediaPlayer>, IMediaPlayersViewModel
    {
        private readonly Func<IMediaPlayer> _playerFactory;
        private readonly AudioDevices _devices;
        private readonly DialogViewModel _dialog;
        private readonly Func<IMediaRepository> _repositoryFactory;
        private readonly IMediaPlayerMapper _mediaPlayerMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaPlayers"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="playerFactory">The player factory.</param>
        /// <param name="repo">The repo.</param>
        /// <param name="devices">The devices.</param>
        /// <param name="dialog">The dialog.</param>
        public MediaPlayers(IMapleLog log, ILocalizationService translationService, IMediaPlayerMapper mediaPlayerMapper, Func<IMediaPlayer> playerFactory, Func<IMediaRepository> repo, AudioDevices devices, DialogViewModel dialog, ISequenceProvider sequenceProvider)
            : base(log, translationService, sequenceProvider)
        {
            _playerFactory = playerFactory ?? throw new ArgumentNullException(nameof(playerFactory));
            _devices = devices ?? throw new ArgumentNullException(nameof(devices));
            _dialog = dialog ?? throw new ArgumentNullException(nameof(dialog));
            _repositoryFactory = repo ?? throw new ArgumentNullException(nameof(repo));
            _mediaPlayerMapper = mediaPlayerMapper ?? throw new ArgumentNullException(nameof(mediaPlayerMapper));
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public override void Load()
        {
            _log.Info($"{_translationService.Translate(nameof(Resources.Loading))} {_translationService.Translate(nameof(Resources.MediaPlayers))}");
            Items.Clear();

            using (var context = _repositoryFactory())
            {
                var main = context.GetMainMediaPlayer();

                Items.Add(main);
                SelectedItem = main;

                Items.AddRange(context.GetAllOptionalMediaPlayers());
            }

            OnLoaded();
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public override void Save()
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

        public override Task SaveAsync()
        {
            return Task.Run(() => Save());
        }

        public override async Task LoadAsync()
        {
            _log.Info($"{_translationService.Translate(nameof(Resources.Loading))} {_translationService.Translate(nameof(Resources.MediaPlayers))}");
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
