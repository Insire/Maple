using System;
using System.Linq;
using System.Threading.Tasks;
using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple.Core
{
    public class MediaItems : VirtualizationListViewModel<MediaItem, MediaItemModel, int>, IMediaItemsViewModel
    {
        private readonly Func<IMediaRepository> _repositoryFactory;
        private readonly IMediaItemMapper _mediaItemMapper;

        public MediaItems(ViewModelServiceContainer container, IMediaItemMapper mediaItemMapper, Func<IMediaRepository> repositoryFactory)
            : base(container)
        {
            _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory), $"{nameof(repositoryFactory)} {Resources.IsRequired}");
            _mediaItemMapper = mediaItemMapper ?? throw new ArgumentNullException(nameof(mediaItemMapper), $"{nameof(mediaItemMapper)} {Resources.IsRequired}");
        }

        public void Add(Playlist playlist)
        {
            var sequence = _sequenceProvider.Get(Items.Select(p => (ISequence)p).ToList());
            Add(_mediaItemMapper.GetNewMediaItem(sequence, playlist));
        }

        public override async Task LoadAsync()
        {
            _log.Info($"{_translationService.Translate(nameof(Resources.Loading))} {_translationService.Translate(nameof(Resources.MediaItems))}");
            Clear();

            using (var context = _repositoryFactory())
            {
                var result = await context.GetMediaItemsAsync().ConfigureAwait(true);
                AddRange(result);
            }

            SelectedItem = Items.FirstOrDefault();
            IsLoaded = true;
        }

        public override async Task SaveAsync()
        {
            _log.Info($"{_translationService.Translate(nameof(Resources.Saving))} {_translationService.Translate(nameof(Resources.MediaItems))}");
            using (var context = _repositoryFactory())
            {
                await context.SaveAsync(this).ConfigureAwait(true);
            }
        }
    }
}
