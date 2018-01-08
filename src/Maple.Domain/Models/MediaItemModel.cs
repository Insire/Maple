using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Maple.Domain
{
    [DebuggerDisplay("{Title}, {Sequence}")]
    public class MediaItemModel : BaseModel<int>
    {
        public int PlaylistId { get; set; }
        [ForeignKey(nameof(PlaylistId))]
        public PlaylistModel Playlist { get; set; }

        /// <summary>
        /// Ticks
        /// </summary>
        public long Duration { get; set; }
        public int PrivacyStatus { get; set; }
        public int MediaItemType { get; set; }
        public string Description { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [Required]
        [MaxLength(2048)]
        public string Location { get; set; }
    }
}
