using Maple.Core;
using Maple.Localization.Properties;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Maple
{
    public class MediaItems : BaseDataListViewModel<MediaItem, Data.MediaItem>, ISaveableViewModel, IMediaItemsViewModel
    {
        private readonly Func<IMediaRepository> _repositoryFactory;
        private readonly IMediaItemMapper _mediaItemMapper;

        public MediaItems(IMapleLog log, ILocalizationService translationService, ISequenceProvider sequenceProvider, IMediaItemMapper mediaItemMapper, Func<IMediaRepository> repo)
            : base(log, translationService, sequenceProvider)
        {
            _repositoryFactory = repo ?? throw new ArgumentNullException(nameof(repo));
            _mediaItemMapper = mediaItemMapper ?? throw new ArgumentNullException(nameof(mediaItemMapper));
        }

        public void Add(int playlistId)
        {
            var sequence = _sequenceProvider.Get(Items.Select(p => (ISequence)p).ToList());
            Add(_mediaItemMapper.GetNewMediaItem(sequence, playlistId));
        }

        public override void Load()
        {
            _log.Info($"{_translationService.Translate(nameof(Resources.Loading))} {_translationService.Translate(nameof(Resources.MediaItems))}");
            Clear();

            using (var context = _repositoryFactory())
                AddRange(context.GetAllMediaItems());

            SelectedItem = Items.FirstOrDefault();
            OnLoaded();
        }

        public override void Save()
        {
            _log.Info($"{_translationService.Translate(nameof(Resources.Saving))} {_translationService.Translate(nameof(Resources.MediaItems))}");
            using (var context = _repositoryFactory())
            {
                context.Save(this);
            }
        }

        public override async Task LoadAsync()
        {
            _log.Info($"{_translationService.Translate(nameof(Resources.Loading))} {_translationService.Translate(nameof(Resources.MediaItems))}");
            Clear();

            using (var context = _repositoryFactory())
            {
                var result = await context.GetAllMediaItemsAsync();
                AddRange(result);
            }

            SelectedItem = Items.FirstOrDefault();
            OnLoaded();
        }

        public override Task SaveAsync()
        {
            return Task.Run(() => Save());
        }
    }
}
