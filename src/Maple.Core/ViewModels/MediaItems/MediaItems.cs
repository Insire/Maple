using System;
using System.Linq;
using System.Threading.Tasks;
using Maple.Domain;

namespace Maple.Core
{
    public class MediaItems : ValidableBaseDataListViewModel<MediaItem, MediaItemModel, int>, IMediaItemsViewModel
    {
        private readonly Func<IMediaRepository> _repositoryFactory;
        private readonly IMediaItemMapper _mediaItemMapper;

        protected MediaItems(ViewModelServiceContainer container, IMapleRepository<MediaItemModel, int> repository)
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

        public void Add(Playlist playlist)
        {
            var sequence = _sequenceProvider.Get(Items.Select(p => (ISequence)p).ToList());
            Add(_mediaItemMapper.GetNewMediaItem(sequence, playlist));
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

        //public override async Task GetCountAsync()
        //{
        //    _log.Info($"{_translationService.Translate(nameof(Resources.Loading))} {_translationService.Translate(nameof(Resources.MediaItems))}");
        //    Clear();

        //    using (var context = _repositoryFactory())
        //    {
        //        var result = await context.GetMediaItemsAsync().ConfigureAwait(true);
        //        AddRange(result);
        //    }

        //    SelectedItem = Items.FirstOrDefault();
        //    IsLoaded = true;
        //}

        //public override async Task SaveAsync()
        //{
        //    _log.Info($"{_translationService.Translate(nameof(Resources.Saving))} {_translationService.Translate(nameof(Resources.MediaItems))}");
        //    using (var context = _repositoryFactory())
        //    {
        //        await context.SaveAsync(this).ConfigureAwait(true);
        //    }
        //}
    }
}
