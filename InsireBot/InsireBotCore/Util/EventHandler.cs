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

    public delegate void RepeatModeChangedEventHandler(object sender, RepeatModeChangedEventEventArgs e);

    public class RepeatModeChangedEventEventArgs : EventArgs
    {
        public RepeatMode RepeatMode { get; private set; }

        public RepeatModeChangedEventEventArgs(RepeatMode repeatMode)
        {
            RepeatMode = repeatMode;
        }
    }
}
