using System.ComponentModel.DataAnnotations.Schema;

namespace Maple.Data
{
    public class MediaItem : BaseObject
    {
        [ForeignKey(nameof(PlaylistId))]
        public virtual Playlist Playlist { get; set; }
        public int PlaylistId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Ticks
        /// </summary>
        public long Duration { get; set; }

        public string Location { get; set; }
        public int PrivacyStatus { get; set; }
    }
}
