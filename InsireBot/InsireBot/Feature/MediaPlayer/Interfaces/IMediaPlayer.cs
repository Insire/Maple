namespace InsireBot.MediaPlayer
{
    public interface IMediaPlayer<IMediaItem> : IPlaying
    {
        bool CanPlay();
        bool CanNext();
        bool CanPrevious();

        IMediaItem Current { get; set; }

        AudioDevice AudioDevice { get; set; }
        MediaPlayPlaybackType MediaPlayPlaybackType { get; set; }
        int Volume { get; set; }
        bool Silent { get; set; }

        int VolumeMax { get; }
        int VolumeMin { get; }
    }
}
