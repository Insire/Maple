using System;
using System.Collections.Generic;
using InsireBotCore;

namespace InsireBotTests
{
    public class TestDataService : IDataService
    {
        public IEnumerable<IMediaItem> GetCurrentMediaItems()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMediaItem> GetMediaItems()
        {
            yield return new MediaItem("Rusko - Somebody To Love (Sigma Remix)", new Uri(@"https://www.youtube.com/watch?v=nF7wa3j57j0"), new TimeSpan(0, 5, 47));
            yield return new MediaItem("Armin van Buuren feat. Sophie - Virtual Friend", new Uri(@"https://www.youtube.com/watch?v=0ypeOKp0x3k"), new TimeSpan(0, 7, 12));
            yield return new MediaItem("Will & Tim ft. Ephixa - Stone Tower Temple", new Uri("C:\\Users\\Insire\\Downloads\\Will & Tim ft. Ephixa - Stone Tower Temple.mp3"));
            yield return new MediaItem("1-Foreword.flac", new Uri("C:\\Users\\Insire\\Desktop\\1-Foreword.flac"));
        }

        public IMediaPlayer<IMediaItem> GetMediaPlayer()
        {
            return new MockMediaPlayer();
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
            var _devices = WinmmService.GetDevCapsPlayback();

            for (int i = 0; i < _devices.Length; i++)
                yield return new AudioDevice(_devices[i].wMid,
                                            _devices[i].wPid,
                                            _devices[i].vDriverVersion,
                                            _devices[i].ToString(),
                                            _devices[i].dwFormats,
                                            _devices[i].wChannels,
                                            _devices[i].wReserved,
                                            _devices[i].dwSupport);
        }
    }
}
