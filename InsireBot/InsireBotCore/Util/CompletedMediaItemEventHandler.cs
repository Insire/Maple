using System;

namespace InsireBotCore
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
