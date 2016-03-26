using System;
using System.IO;

namespace InsireBot.MediaPlayer
{
    public static class MediaPlayerFactory
    {
        public static IMediaPlayer<IMediaItem> Create(MediaPlayerType mediaPlayerType)
        {
            switch (mediaPlayerType)
            {
                case MediaPlayerType.VLCDOTNET:
                    var settings = new DotNetPlayerSettings
                    {
                        Directory = "",
                        Extension = "",
                        FileName = "",
                        VlcLibDirectory = new DirectoryInfo("C:"),
                        Options = new[]
                        {
                            "--aout=waveout",
                            "--waveout-audio-device={0}",
                            "--ffmpeg-hw",
                            "--no-video"
                        },
                        MediaPlayPlaybackType = MediaPlayPlaybackType.Play

                    };
                    return new DotNetPlayer(settings);

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
