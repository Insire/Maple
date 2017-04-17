using Maple.Core;
using System;
using System.Windows.Media;

namespace Maple
{
    public delegate void CompletedMediaItemEventHandler(object sender, CompletedMediaItemEventEventArgs e);
    public class CompletedMediaItemEventEventArgs : EventArgs
    {
        public IMediaItem MediaItem { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompletedMediaItemEventEventArgs"/> class.
        /// </summary>
        /// <param name="mediaItem">The media item.</param>
        public CompletedMediaItemEventEventArgs(IMediaItem mediaItem)
        {
            MediaItem = mediaItem;
        }
    }

    public delegate void RepeatModeChangedEventHandler(object sender, RepeatModeChangedEventEventArgs e);
    public class RepeatModeChangedEventEventArgs : EventArgs
    {
        public RepeatMode RepeatMode { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatModeChangedEventEventArgs"/> class.
        /// </summary>
        /// <param name="repeatMode">The repeat mode.</param>
        public RepeatModeChangedEventEventArgs(RepeatMode repeatMode)
        {
            RepeatMode = repeatMode;
        }
    }

    public delegate void ShuffleModeChangedEventHandler(object sender, ShuffleModeChangedEventEventArgs e);
    public class ShuffleModeChangedEventEventArgs : EventArgs
    {
        public bool IsShuffeling { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShuffleModeChangedEventEventArgs"/> class.
        /// </summary>
        /// <param name="isShuffeling">if set to <c>true</c> [is shuffeling].</param>
        public ShuffleModeChangedEventEventArgs(bool isShuffeling)
        {
            IsShuffeling = isShuffeling;
        }
    }

    public delegate void PlayingMediaItemEventHandler(object sender, PlayingMediaItemEventArgs e);
    public class PlayingMediaItemEventArgs : EventArgs
    {
        public IMediaItem MediaItem { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayingMediaItemEventArgs"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public PlayingMediaItemEventArgs(IMediaItem item)
        {
            MediaItem = item;
        }
    }

    public delegate void UiPrimaryColorEventHandler(object sender, UiPrimaryColorEventArgs e);
    public class UiPrimaryColorEventArgs : EventArgs
    {
        public Color Color { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="UiPrimaryColorEventArgs"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public UiPrimaryColorEventArgs(Color item)
        {
            Color = item;
        }
    }

    public delegate void AudioDeviceChangedEventHandler(object sender, AudioDeviceChangedEventArgs e);
    public class AudioDeviceChangedEventArgs : EventArgs
    {
        public AudioDevice AudioDevice { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="AudioDeviceChangedEventArgs"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public AudioDeviceChangedEventArgs(AudioDevice item)
        {
            AudioDevice = item;
        }
    }
}
