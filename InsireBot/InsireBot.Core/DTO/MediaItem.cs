using System;

namespace Maple.Core
{
    /// <summary>
    ///
    /// </summary>
    public class MediaItem
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }
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
        /// Ticks
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public long Duration { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public string Location { get; set; }
        /// <summary>
        /// Gets or sets the privacy status.
        /// </summary>
        /// <value>
        /// The privacy status.
        /// </value>
        public int PrivacyStatus { get; set; }
        /// <summary>
        /// Gets or sets the sequence.
        /// </summary>
        /// <value>
        /// The sequence.
        /// </value>
        public int Sequence { get; set; }
        /// <summary>
        /// Gets or sets the playlist identifier.
        /// </summary>
        /// <value>
        /// The playlist identifier.
        /// </value>
        public int PlaylistId { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}
