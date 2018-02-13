using System;
using System.Linq;
using System.Threading.Tasks;
using Maple.Domain;

namespace Maple.Core
{
    public class MediaPlayers : ValidableBaseDataListViewModel<MediaPlayer, MediaPlayerModel, int>, IMediaPlayersViewModel
    {
        private readonly Func<IMediaPlayer> _playerFactory;
        private readonly AudioDevices _devices;
        private readonly IDialogViewModel _dialog;
        private readonly Func<IMediaRepository> _repositoryFactory;
        private readonly IMediaPlayerMapper _mediaPlayerMapper;
        private readonly ILoggingNotifcationService _notificationService;

        protected MediaPlayers(ViewModelServiceContainer container, IMapleRepository<MediaPlayerModel, int> repository)
            : base(container, repository)
        {
        }

        public override bool IsLoaded
        {
            get { throw new NotImplementedException(); }
            protected set => throw new NotImplementedException();
        }

        public override IAsyncCommand SaveCommand => throw new NotImplementedException();

        public override IAsyncCommand LoadCommand => throw new NotImplementedException();

        public override IAsyncCommand RefreshCommand => throw new NotImplementedException();

        public void Add()
        {
            var sequence = _sequenceProvider.Get(Items.Select(p => (ISequence)p).ToList());
            Add(_mediaPlayerMapper.GetNewMediaPlayer(sequence));
        }

        public override Task GetCountAsync()
        {
            throw new NotImplementedException();
        }

        public override Task GetItemsWithKey(int[] keys)
        {
            throw new NotImplementedException();
        }

        public override Task SaveAsync()
        {
            throw new NotImplementedException();
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

        //public override async Task SaveAsync()
        //{
        //    _log.Info($"{_translationService.Translate(nameof(Resources.Saving))} {_translationService.Translate(nameof(Resources.MediaPlayers))}");
        //    using (var context = _repositoryFactory())
        //    {
        //        await context.SaveAsync(this).ConfigureAwait(true);
        //    }
        //}

        //public override async Task GetCountAsync()
        //{
        //    _notificationService.Info($"{_translationService.Translate(nameof(Resources.Loading))} {_translationService.Translate(nameof(Resources.MediaPlayers))}");
        //    Clear();

        //    using (var context = _repositoryFactory())
        //    {
        //        var main = await context.GetMainMediaPlayerAsync().ConfigureAwait(true);

        //        Add(main);
        //        SelectedItem = main;

        //        var others = await context.GetAllOptionalMediaPlayersAsync().ConfigureAwait(true);
        //        AddRange(others);
        //    }

        //    IsLoaded = true;
        //}
    }
}
