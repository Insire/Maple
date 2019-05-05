using System.Diagnostics;

namespace Maple.Domain
{
    [DebuggerDisplay("{Title}, {Sequence}")]
    public class MediaItemModel : BaseObject<int>
    {
        public int PlaylistId { get; set; }

        public virtual PlaylistModel Playlist { get; set; }

        /// <summary>
        /// Ticks
        /// </summary>
        public long Duration { get; set; }

        public int PrivacyStatus { get; set; }
        public int MediaItemType { get; set; }
        public string Description { get; set; }

        public string Title { get; set; }

        public string Location { get; set; }
    }
}
