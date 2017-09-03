namespace Maple
{
    public interface IPlaylistMapper : IBaseMapper<Playlist, Data.Playlist>
    {
        Playlist GetNewPlaylist(int sequence);
    }
}
