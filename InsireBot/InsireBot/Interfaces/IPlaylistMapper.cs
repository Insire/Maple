namespace InsireBot
{
    public interface IPlaylistMapper
    {
        Core.Playlist GetCore(PlaylistViewModel mediaitem);

        Core.Playlist GetCore(Data.Playlist mediaitem);

        Data.Playlist GetData(Core.Playlist mediaitem);

        Data.Playlist GetData(PlaylistViewModel mediaitem);

        PlaylistViewModel Get(Data.Playlist mediaitem);

        PlaylistViewModel Get(Core.Playlist mediaitem);
    }
}
