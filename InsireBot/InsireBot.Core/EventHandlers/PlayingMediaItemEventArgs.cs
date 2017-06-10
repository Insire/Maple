using System;

namespace Maple.Core
{
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
}
