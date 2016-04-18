using System;

namespace InsireBotCore
{
    public interface IMediaPlayer<IMediaItem> : IPlaying, IDisposable
    {
        event CompletedMediaItemEventHandler CompletedMediaItem;

        bool Disposed { get; }
        bool IsPlaying { get; }

        void Play();
        void Next();
        void Previous();

        IPlaylist Playlist { get;}

        AudioDevice AudioDevice { get; set; }
        int Volume { get; set; }
        bool Silent { get; set; }

        int VolumeMax { get; }
        int VolumeMin { get; }
    }
}
