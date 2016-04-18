using System;
using System.Collections.Generic;

namespace InsireBotCore
{
    public class DesignTimeDataService : IDataService
    {
        public IEnumerable<IMediaItem> GetCurrentMediaItems()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMediaItem> GetMediaItems()
        {
            yield return new MediaItem("Rusko - Somebody To Love (Sigma Remix)", new Uri(@"https://www.youtube.com/watch?v=nF7wa3j57j0"), new TimeSpan(0, 5, 47));
            yield return new MediaItem("Armin van Buuren feat. Sophie - Virtual Friend", new Uri(@"https://www.youtube.com/watch?v=0ypeOKp0x3k"), new TimeSpan(0, 7, 12));
        }

        public IMediaPlayer<IMediaItem> GetMediaPlayer()
        {
            throw new NotImplementedException();
        }

        public ISettings GetMediaPlayerSettings()
        {
            throw new NotImplementedException();
        }

        public MediaPlayerType GetMediaPlayerType()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AudioDevice> GetPlaybackDevices()
        {
            yield return new AudioDevice(2, 2, 2, "TestDevice #1", 2, 2, 2, 2);
            yield return new AudioDevice(2, 2, 2, "TestDevice #2", 2, 2, 2, 2);
        }
    }
}
