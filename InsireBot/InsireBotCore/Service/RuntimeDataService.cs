using System;
using System.Collections.Generic;
using System.IO;
using InsireBotCore;

namespace InsireBotCore
{
    public class RuntimeDataService : IDataService
    {
        public IEnumerable<IMediaItem> GetMediaItems()
        {
            throw new NotImplementedException();
        }

        public IMediaPlayer<IMediaItem> GetMediaPlayer()
        {
            var vlcInstallDirectory = string.Empty;
            // vlc is a 32bit application, so we always want to get the base 32bit install directory, regardless of OS architecture
            if (Environment.Is64BitOperatingSystem)
                vlcInstallDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "VideoLAN\\VLC");
            else
                vlcInstallDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "VideoLAN\\VLC");

            var settings = new DotNetPlayerSettings
            {
                Directory = "C:",
                Extension = "exe",
                FileName = "vlc",
                VlcLibDirectory = new DirectoryInfo(vlcInstallDirectory),
                Options = new[]
                {
                            "--aout=waveout",
                            "--waveout-audio-device={0}",
                            "--ffmpeg-hw",
                            "--no-video"
                        },
                RepeatMode = RepeatMode.None,

            };

            return new DotNetPlayer(this, settings);
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
