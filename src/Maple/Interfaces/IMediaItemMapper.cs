using Maple.Domain;

namespace Maple
{
    public interface IMediaItemMapper : IBaseMapper<MediaItem, MediaItemModel>
    {
        MediaItem GetNewMediaItem(int sequence, Playlist playlist);

        MediaItemModel GetDataNewMediaItem(PlaylistModel playlist);
    }
}
