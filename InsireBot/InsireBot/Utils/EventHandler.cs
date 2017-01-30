using InsireBot.Core;
using System;
using System.Windows.Media;

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

    public delegate void PlayingMediaItemEventHandler(object sender, PlayingMediaItemEventArgs e);
    public class PlayingMediaItemEventArgs : EventArgs
    {
        public IMediaItem MediaItem { get; private set; }
        public PlayingMediaItemEventArgs(IMediaItem item)
        {
            MediaItem = item;
        }
    }

    public delegate void UiPrimaryColorEventHandler(object sender, PlayingMediaItemEventArgs e);
    public class UiPrimaryColorEventArgs : EventArgs
    {
        public Color Color { get; private set; }
        public UiPrimaryColorEventArgs(Color item)
        {
            Color = item;
        }
    }
}
