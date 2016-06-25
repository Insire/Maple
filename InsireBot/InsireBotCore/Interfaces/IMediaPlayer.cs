using System;

namespace InsireBotCore
{
    public interface IMediaPlayer<T> : IPlaying, IDisposable where T : IMediaItem
    {
        event CompletedMediaItemEventHandler CompletedMediaItem;

        bool Disposed { get; }
        bool IsPlaying { get; }

        void Play();
        void Next();
        void Previous();

        Playlist<T> Playlist { get;}

        AudioDevice AudioDevice { get; set; }
        int Volume { get; set; }
        bool Silent { get; set; }

        int VolumeMax { get; }
        int VolumeMin { get; }
    }
}
