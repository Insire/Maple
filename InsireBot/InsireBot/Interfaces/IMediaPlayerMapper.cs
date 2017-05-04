namespace Maple
{
    public interface IMediaPlayerMapper
    {
        MediaPlayer Get(Data.MediaPlayer player, Playlist playlist);
        MediaPlayer GetNewMediaPlayer(int sequence, Playlist playlist = null);
    }
}