using System;

namespace InsireBotCore
{
    public interface IMediaPlayer<IMediaItem> : IPlaying, IDisposable
    {
        event CompletedMediaItemEventHandler CompletedMediaItem;

        bool Disposed { get; }
        bool IsPlaying { get; }
        bool IsShuffling { get; set; }
        void Play(IMediaItem mediaItem);

        IMediaItem Current { get; set; }

        AudioDevice AudioDevice { get; set; }
        RepeatMode RepeatMode { get; set; }
        int Volume { get; set; }
        bool Silent { get; set; }

        int VolumeMax { get; }
        int VolumeMin { get; }
    }
}
