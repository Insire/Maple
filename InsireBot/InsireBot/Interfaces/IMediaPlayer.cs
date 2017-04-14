using Maple.Core;
using System;

namespace Maple
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IMediaPlayer : IDisposable
    {
        /// <summary>
        /// Occurs when [completed media item].
        /// </summary>
        event CompletedMediaItemEventHandler CompletedMediaItem;
        /// <summary>
        /// Occurs when [playing media item].
        /// </summary>
        event PlayingMediaItemEventHandler PlayingMediaItem;
        /// <summary>
        /// Occurs when [audio device changed].
        /// </summary>
        event AudioDeviceChangedEventHandler AudioDeviceChanged;
        /// <summary>
        /// Occurs when [audio device changing].
        /// </summary>
        event EventHandler AudioDeviceChanging;

        /// <summary>
        /// Gets a value indicating whether this <see cref="IMediaPlayer"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        bool Disposed { get; }
        /// <summary>
        /// Gets a value indicating whether this instance is playing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is playing; otherwise, <c>false</c>.
        /// </value>
        bool IsPlaying { get; }

        /// <summary>
        /// Plays the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        void Play(IMediaItem item);
        /// <summary>
        /// Pauses this instance.
        /// </summary>
        void Pause();
        /// <summary>
        /// Stops this instance.
        /// </summary>
        void Stop();

        /// <summary>
        /// Determines whether this instance can stop.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can stop; otherwise, <c>false</c>.
        /// </returns>
        bool CanStop();
        /// <summary>
        /// Determines whether this instance can pause.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can pause; otherwise, <c>false</c>.
        /// </returns>
        bool CanPause();
        /// <summary>
        /// Determines whether this instance can play the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if this instance can play the specified item; otherwise, <c>false</c>.
        /// </returns>
        bool CanPlay(IMediaItem item);

        /// <summary>
        /// Gets or sets the volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        int Volume { get; set; }
        /// <summary>
        /// Gets the volume maximum.
        /// </summary>
        /// <value>
        /// The volume maximum.
        /// </value>
        int VolumeMax { get; }
        /// <summary>
        /// Gets the volume minimum.
        /// </summary>
        /// <value>
        /// The volume minimum.
        /// </value>
        int VolumeMin { get; }
        /// <summary>
        /// Gets or sets the audio device.
        /// </summary>
        /// <value>
        /// The audio device.
        /// </value>
        AudioDevice AudioDevice { get; set; }
    }
}
