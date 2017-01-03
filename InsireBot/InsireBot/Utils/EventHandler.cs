using System;

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

    public delegate void RepeatModeChangedEventHandler(object sender, RepeatModeChangedEventEventArgs e);
    public class RepeatModeChangedEventEventArgs : EventArgs
    {
        public RepeatMode RepeatMode { get; private set; }

        public RepeatModeChangedEventEventArgs(RepeatMode repeatMode)
        {
            RepeatMode = repeatMode;
        }
    }

    public delegate void ShuffleModeChangedEventHandler(object sender, ShuffleModeChangedEventEventArgs e);
    public class ShuffleModeChangedEventEventArgs : EventArgs
    {
        public bool IsShuffeling { get; private set; }

        public ShuffleModeChangedEventEventArgs(bool isShuffeling)
        {
            IsShuffeling = isShuffeling;
        }
    }
}
