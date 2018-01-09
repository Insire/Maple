namespace Maple.Core
{
    public class MediaItemDataProvider : BaseDataProvider<MediaItem, int>
    {
        protected MediaItemDataProvider(IMediaRepository mediaRepository, ICachingService<int, MediaItem> cache)
            : base(mediaRepository.GetMediaItemByIdAsync, cache)
        {
        }
    }
}
