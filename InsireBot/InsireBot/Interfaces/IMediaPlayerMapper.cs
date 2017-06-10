namespace Maple
{
    public interface IMediaPlayerMapper : IBaseMapper<MediaPlayer, Core.MediaPlayer, Data.MediaPlayer>
    {
        MainMediaPlayer GetMain(Data.MediaPlayer player, Playlist playlist);

        MediaPlayer Get(Data.MediaPlayer player, Playlist playlist);
        MediaPlayer GetNewMediaPlayer(int sequence, Playlist playlist = null);
    }
}