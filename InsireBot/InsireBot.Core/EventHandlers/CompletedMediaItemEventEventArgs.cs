using System;

namespace Maple.Core
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
}
