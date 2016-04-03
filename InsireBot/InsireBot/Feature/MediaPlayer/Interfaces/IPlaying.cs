namespace InsireBot.MediaPlayer
{
    public interface IPlaying
    {
        void Play(IMediaItem item);
        void Pause();
        void Stop();
    }
}
