using System;
using InsireBotCore;

namespace InsireBotTests
{
    public class MockMediaPlayer : IMediaPlayer<IMediaItem>
    {
        public AudioDevice AudioDevice { get; set; }

        public bool CanPlay
        {
            get
            {
                return true;
            }
        }

        public bool Disposed { get; set; }

        public bool IsPlaying { get; set; }

        public Playlist<IMediaItem> Playlist { get; set; }

        public bool Silent { get; set; }

        public int Volume { get; set; }

        public int VolumeMax { get; set; }

        public int VolumeMin { get; set; }

        public event CompletedMediaItemEventHandler CompletedMediaItem;

        public MockMediaPlayer(IDataService dataService)
        {
            Playlist = new Playlist<IMediaItem>();
            Playlist.AddRange(dataService.GetMediaItems());

            VolumeMin = 0;
            VolumeMax = 100;


        }

        public void Dispose()
        {
            Disposed = true;
        }

        public void Next()
        {
            var next = Playlist.Next();
            Playlist.Set(next);
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Play()
        {
            CompletedMediaItem?.Invoke(this, new CompletedMediaItemEventEventArgs(Playlist.CurrentItem));
        }

        public void Play(IMediaItem mediaItem)
        {
            Playlist.Set(mediaItem);
        }

        public void Previous()
        {
            var previous = Playlist.Previous();
            Playlist.Set(previous);
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
