using System;
using System.IO;

namespace InsireBot.MediaPlayer
{
    public static class MediaPlayerFactory
    {
        public static IMediaPlayer<IMediaItem> Create(IDataService dataService, MediaPlayerType mediaPlayerType)
        {
            switch (mediaPlayerType)
            {
                case MediaPlayerType.VLCDOTNET:
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
                    return new DotNetPlayer(dataService, settings);

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
