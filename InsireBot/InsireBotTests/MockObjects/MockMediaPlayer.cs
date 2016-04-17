using System;
using InsireBotCore;

namespace InsireBotTests
{
    public class MockMediaPlayer : IMediaPlayer<IMediaItem>
    {
        public AudioDevice AudioDevice { get; set; }

        public IMediaItem Current { get; set; }

        public bool Disposed { get; }

        public bool IsPlaying { get; }

        public bool IsShuffling { get; set; }

        public RepeatMode RepeatMode { get; set; }

        public bool Silent { get; set; }

        public int Volume { get; set; }

        public int VolumeMax { get; }

        public int VolumeMin { get; }

        public event CompletedMediaItemEventHandler CompletedMediaItem;
        public event RepeatModeChangedEventHandler RepeatModeChanged;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Play(IMediaItem mediaItem)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
