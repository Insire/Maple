namespace Maple
{
    public interface IMediaPlayerMapper
    {
        MediaPlayer Get(Data.MediaPlayer player, Playlist playlist);
    }
}