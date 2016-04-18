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
            throw new NotImplementedException();
        }
    }
}
