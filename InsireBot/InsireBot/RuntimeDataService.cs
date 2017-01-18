using System;
using System.Collections.Generic;

namespace InsireBot
{
    public class RuntimeDataService : IDataService
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IEnumerable<IMediaItem> GetMediaItems()
        {
            return new IMediaItem[0];
        }

        public ISettings GetMediaPlayerSettings()
        {
            _log.Info("Loading MediaPlayer Settings");

            var mediaPlayerType = GetMediaPlayerType();
            switch (mediaPlayerType)
            {
                case MediaPlayerType.NAUDIO:
                    return new NAudioPlayerSettings();

                default:
                    throw new NotImplementedException(nameof(mediaPlayerType));
            }
        }

        public IMediaPlayer<IMediaItem> GetMediaPlayer()
        {
            _log.Info("Loading MediaPlayer");

            var mediaPlayerType = GetMediaPlayerType();
            switch (mediaPlayerType)
            {
                case MediaPlayerType.NAUDIO:
                    return new NAudioMediaPlayer(this);

                default:
                    throw new NotImplementedException(nameof(mediaPlayerType));
            }
        }

        public IEnumerable<AudioDevice> GetPlaybackDevices()
        {
            _log.Info("Loading PlaybackDevices");

            var mediaPlayerType = GetMediaPlayerType();

            switch (mediaPlayerType)
            {
                case MediaPlayerType.NAUDIO:
                    var devices = PlaybackDeviceFactory.GetAudioDevices();
                    foreach (var device in devices)
                        yield return device;
                    break;
            }
        }

        public MediaPlayerType GetMediaPlayerType()
        {
            return MediaPlayerType.NAUDIO;
        }

        public IEnumerable<IMediaItem> GetCurrentMediaItems()
        {
            _log.Info("Loading Playlist");
            //TODO add DB access
            yield return new MediaItem("Rusko - Somebody To Love (Sigma Remix)", new Uri(@"https://www.youtube.com/watch?v=nF7wa3j57j0"), new TimeSpan(0, 5, 47));
            yield return new MediaItem("Armin van Buuren feat. Sophie - Virtual Friend", new Uri(@"https://www.youtube.com/watch?v=0ypeOKp0x3k"), new TimeSpan(0, 7, 12));
            yield return new MediaItem("Will & Tim ft. Ephixa - Stone Tower Temple", new Uri("C:\\Users\\Insire\\Downloads\\Will & Tim ft. Ephixa - Stone Tower Temple.mp3"));
            yield return new MediaItem("1-Foreword.flac", new Uri("C:\\Users\\Insire\\Desktop\\1-Foreword.flac"));
        }
    }
}
