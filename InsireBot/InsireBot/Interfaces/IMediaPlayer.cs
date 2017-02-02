using Maple.Core;
using System;

namespace Maple
{
    public interface IMediaPlayer: IDisposable
    {
        event CompletedMediaItemEventHandler CompletedMediaItem;
        event PlayingMediaItemEventHandler PlayingMediaItem;
        event AudioDeviceChangedEventHandler AudioDeviceChanged;
        event EventHandler AudioDeviceChanging;

        bool Disposed { get; }
        bool IsPlaying { get; }

        void Play(IMediaItem item);
        void Pause();
        void Stop();

        bool CanStop();
        bool CanPause();
        bool CanPlay(IMediaItem item);

        int Volume { get; set; }
        int VolumeMax { get; }
        int VolumeMin { get; }
        AudioDevice AudioDevice { get; set; }
    }
}
