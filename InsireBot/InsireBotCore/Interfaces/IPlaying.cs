namespace InsireBotCore
{
    public interface IPlaying
    {
        void Play(IMediaItem item);
        void Pause();
        void Stop();
    }
}
