using Maple.Domain;

namespace Maple.Core
{
    public interface IPlaylistMapper : IBaseMapper<Playlist, PlaylistModel, int>
    {
        Playlist GetNewPlaylist();
    }
}
