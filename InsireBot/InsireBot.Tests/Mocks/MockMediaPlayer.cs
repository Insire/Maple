using Maple.Core;
using System;

namespace Maple.Tests
{
    public class MockMediaPlayer : IMediaPlayer
    {
        public AudioDevice AudioDevice { get; set; }

        public bool Disposed { get; set; }

        public bool IsPlaying { get; set; }

        public int Volume { get; set; }

        public int VolumeMax { get; set; }

        public int VolumeMin { get; set; }

        public event AudioDeviceChangedEventHandler AudioDeviceChanged;
        public event EventHandler AudioDeviceChanging;
        public event CompletedMediaItemEventHandler CompletedMediaItem;
        public event PlayingMediaItemEventHandler PlayingMediaItem;

        public bool CanPause()
        {
            return true;
        }

        public bool CanPlay(IMediaItem item)
        {
            return true;
        }

        public bool CanStop()
        {
            return true;
        }

        public void Dispose()
        {

        }

        public void Pause()
        {

        }

        public void Play(IMediaItem item)
        {

        }

        public void Stop()
        {

        }
    }
}
