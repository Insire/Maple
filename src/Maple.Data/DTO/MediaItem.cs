using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Maple.Data
{
    [DebuggerDisplay("{Title}, {Sequence}")]
    public class MediaItem : BaseObject
    {
        public int PlaylistId { get; set; }
        [ForeignKey(nameof(PlaylistId))]
        public Playlist Playlist { get; set; }

        public Raw Raw { get; set; }

        /// <summary>
        /// Ticks
        /// </summary>
        public long Duration { get; set; }
        public int PrivacyStatus { get; set; }
        public string Description { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [Required]
        [MaxLength(2048)]
        public string Location { get; set; }
    }
}
