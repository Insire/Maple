using Maple.Domain;

namespace Maple
{
    public interface IMediaPlayerMapper : IBaseMapper<MediaPlayer, MediaPlayerModel, int>
    {
        MainMediaPlayer GetMain(MediaPlayerModel player, Playlist playlist);

        MediaPlayer Get(MediaPlayerModel player, Playlist playlist);
        MediaPlayer GetNewMediaPlayer(int sequence, Playlist playlist = null);
    }
}