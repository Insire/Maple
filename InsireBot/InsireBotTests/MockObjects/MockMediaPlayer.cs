using System;
using InsireBotCore;

namespace InsireBotTests
{
    public class MockMediaPlayer : IMediaPlayer<IMediaItem>
    {
        public AudioDevice AudioDevice { get; set; }

        public bool Disposed { get; set; }

        public bool IsPlaying { get; set; }

        public IPlaylist<IMediaItem> Playlist { get; set; }

        public bool Silent { get; set; }

        public int Volume { get; set; }

        public int VolumeMax { get; set; }

        public int VolumeMin { get; set; }

        public event CompletedMediaItemEventHandler CompletedMediaItem;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Next()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Play()
        {
            throw new NotImplementedException();
        }

        public void Play(IMediaItem mediaItem)
        {
            throw new NotImplementedException();
        }

        public void Previous()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
