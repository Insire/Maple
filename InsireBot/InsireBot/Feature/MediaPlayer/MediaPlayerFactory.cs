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
                    var settings = new DotNetPlayerSettings
                    {
                        Directory = "C:",
                        Extension = "exe",
                        FileName = "vlc",
                        VlcLibDirectory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "VideoLAN\\VLC")),
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
