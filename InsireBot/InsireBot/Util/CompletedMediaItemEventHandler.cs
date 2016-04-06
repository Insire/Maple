using System;
using InsireBot.MediaPlayer;

namespace InsireBot
{
    public delegate void CompletedMediaItemEventHandler(object sender, CompletedMediaItemEventEventArgs e);

    public class CompletedMediaItemEventEventArgs : EventArgs
    {
        public IMediaItem MediaItem { get; private set; }

        public CompletedMediaItemEventEventArgs(IMediaItem mediaItem)
        {
            MediaItem = mediaItem;
        }
    }
}
