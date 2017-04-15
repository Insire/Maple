using System;
using System.Collections.Generic;

namespace Maple.Core
{
    /// <summary>
    ///
    /// </summary>
    public class Playlist
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public string Location { get; set; }
        /// <summary>
        /// Gets or sets the repeat mode.
        /// </summary>
        /// <value>
        /// The repeat mode.
        /// </value>
        public int RepeatMode { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is shuffeling.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is shuffeling; otherwise, <c>false</c>.
        /// </value>
        public bool IsShuffeling { get; set; }
        /// <summary>
        /// Gets or sets the privacy status.
        /// </summary>
        /// <value>
        /// The privacy status.
        /// </value>
        public int PrivacyStatus { get; set; }
        /// <summary>
        /// Gets or sets the media items.
        /// </summary>
        /// <value>
        /// The media items.
        /// </value>
        public List<MediaItem> MediaItems { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}
