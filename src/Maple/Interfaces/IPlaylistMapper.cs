using Maple.Domain;

namespace Maple
{
    public interface IPlaylistMapper : IBaseMapper<Playlist, PlaylistModel>
    {
        Playlist GetNewPlaylist(int sequence);
    }
}
