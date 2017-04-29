using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maple.Data
{
    public class MediaItem : BaseObject
    {
        [ForeignKey(nameof(PlaylistId))]
        public virtual Playlist Playlist { get; set; }
        public int PlaylistId { get; set; }

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Ticks
        /// </summary>
        public long Duration { get; set; }

        [Required]
        public string Location { get; set; }
        public int PrivacyStatus { get; set; }

        [ForeignKey(nameof(RawId))]
        public virtual Raw Raw { get; set; }
        public int RawId { get; set; }
    }
}
