using System;

namespace InsireBot.MediaPlayer
{
    public interface IMediaPlayer<IMediaItem> : IPlaying, IDisposable
    {
        bool Disposed { get; }

        bool CanPlay();
        bool CanNext();
        bool CanPrevious();

        IMediaItem Current { get; set; }

        AudioDevice AudioDevice { get; set; }
        RepeatMode RepeatMode { get; set; }
        int Volume { get; set; }
        bool Silent { get; set; }

        int VolumeMax { get; }
        int VolumeMin { get; }
    }
}
