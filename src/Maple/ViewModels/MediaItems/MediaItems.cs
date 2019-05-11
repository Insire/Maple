using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Maple.Core;
using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple
{
    public sealed class MediaItems : BusinessListViewModel<MediaItem, MediaItemModel>, IMediaItemsViewModel
    {
        private readonly Func<IUnitOfWork> _repositoryFactory;
        private readonly IMediaItemMapper _mediaItemMapper;

        public MediaItems(ViewModelServiceContainer container, IMediaItemMapper mediaItemMapper, Func<IUnitOfWork> repositoryFactory)
            : base(container)
        {
            _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory), $"{nameof(repositoryFactory)} {Resources.IsRequired}");
            _mediaItemMapper = mediaItemMapper ?? throw new ArgumentNullException(nameof(mediaItemMapper), $"{nameof(mediaItemMapper)} {Resources.IsRequired}");
        }

        public void Add(Playlist playlist)
        {
            var sequence = _sequenceProvider.Get(Items.Cast<ISequence>().ToList());
            Add(_mediaItemMapper.GetNewMediaItem(sequence, playlist));
        }

        private async Task SaveInternal()
        {
            _log.Info($"{_translationService.Translate(nameof(Resources.Saving))} {_translationService.Translate(nameof(Resources.MediaItems))}");
            using (var context = _repositoryFactory())
            {
                foreach (var item in Items)
                    context.MediaItemRepository.Update(item.Model);

                await context.SaveChanges().ConfigureAwait(false);
            }
        }

        public override async Task Load()
        {
            _log.Info($"{_translationService.Translate(nameof(Resources.Loading))} {_translationService.Translate(nameof(Resources.MediaItems))}");
            Clear();

            using (var context = _repositoryFactory())
            {
                var result = await context.MediaItemRepository.ReadAsync().ConfigureAwait(true);
                if (result.Count > 0)
                {
                    AddRange(_mediaItemMapper.GetMany(result));
                }
            }

            SelectedItem = Items.FirstOrDefault();
            OnLoaded();
        }

        public override Task Save()
        {
            return SaveInternal();
        }

        protected override Task Load(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        protected override Task Unload(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        protected override Task Refresh(CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
