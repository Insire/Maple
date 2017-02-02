namespace Maple
{
    public interface IPlaylistMapper
    {
        Core.Playlist GetCore(Playlist mediaitem);

        Core.Playlist GetCore(Data.Playlist mediaitem);

        Data.Playlist GetData(Core.Playlist mediaitem);

        Data.Playlist GetData(Playlist mediaitem);

        Playlist Get(Data.Playlist mediaitem);

        Playlist Get(Core.Playlist mediaitem);
    }
}
