using Maple.Domain;

namespace Maple
{
    public interface IPlaylistMapper : IBaseMapper<Playlist, PlaylistModel, int>
    {
        Playlist GetNewPlaylist();
    }
}
