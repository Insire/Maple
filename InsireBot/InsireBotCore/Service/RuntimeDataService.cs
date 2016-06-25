using System;
using System.Collections.Generic;
using System.IO;

namespace InsireBotCore
{
    public class RuntimeDataService : IDataService
    {
        public IEnumerable<IMediaItem> GetMediaItems()
        {
            return new IMediaItem[0];
        }

        public ISettings GetMediaPlayerSettings()
        {
            var mediaPlayerType = GetMediaPlayerType();
            switch (mediaPlayerType)
            {
                case MediaPlayerType.VLCDOTNET:
                    var vlcInstallDirectory = string.Empty;
                    // TODO change this depending on app architecture
                    // vlc is a 32bit application, so we always want to get the base 32bit install directory, regardless of OS architecture
                    if (Environment.Is64BitOperatingSystem)
                        vlcInstallDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "VideoLAN\\VLC");
                    else
                        vlcInstallDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "VideoLAN\\VLC");

                    return new DotNetPlayerSettings
                    {
                        Directory = new DirectoryInfo(vlcInstallDirectory),
                        FileName = "vlc",
                        Options = new[]
                        {
                            "--aout=waveout",
                            "--waveout-audio-device={0}",
                            "--ffmpeg-hw",
                            "--no-video"
                        },
                        RepeatMode = RepeatMode.None,
                    };

                default:
                    throw new NotImplementedException(nameof(mediaPlayerType));
            }
        }

        public IMediaPlayer<IMediaItem> GetMediaPlayer()
        {
            return new DotNetPlayer(this);
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

        public MediaPlayerType GetMediaPlayerType()
        {
            return MediaPlayerType.VLCDOTNET;
        }

        public IEnumerable<IMediaItem> GetCurrentMediaItems()
        {
            //TODO add DB access
            yield return new MediaItem("Rusko - Somebody To Love (Sigma Remix)", new Uri(@"https://www.youtube.com/watch?v=nF7wa3j57j0"), new TimeSpan(0, 5, 47));
            yield return new MediaItem("Armin van Buuren feat. Sophie - Virtual Friend", new Uri(@"https://www.youtube.com/watch?v=0ypeOKp0x3k"), new TimeSpan(0, 7, 12));
            yield return new MediaItem("Will & Tim ft. Ephixa - Stone Tower Temple", new Uri("C:\\Users\\Insire\\Downloads\\Will & Tim ft. Ephixa - Stone Tower Temple.mp3"));
            yield return new MediaItem("1-Foreword.flac", new Uri("C:\\Users\\Insire\\Desktop\\1-Foreword.flac"));
        }
    }
}
