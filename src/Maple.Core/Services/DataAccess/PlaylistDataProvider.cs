namespace Maple.Core
{
    public class PlaylistDataProvider : BaseDataProvider<Playlist, int>
    {
        protected PlaylistDataProvider(IMediaRepository mediaRepository, ICachingService<int, Playlist> cache)
            : base(mediaRepository.GetPlaylistByIdAsync, cache)
        {
        }
    }
}
