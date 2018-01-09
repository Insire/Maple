namespace Maple.Core
{
    public class MediaPlayerDataProvider : BaseDataProvider<MediaPlayer, int>
    {
        protected MediaPlayerDataProvider(IMediaRepository mediaRepository, ICachingService<int, MediaPlayer> cache)
            : base(mediaRepository.GetMediaPlayerByIdAsync, cache)
        {
        }
    }
}
