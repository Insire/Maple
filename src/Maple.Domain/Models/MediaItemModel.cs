using System;
using System.Diagnostics;

namespace Maple.Domain
{
    [DebuggerDisplay("MediaItemModel: {Sequence}, {Name}")]
    public class MediaItemModel : Entity<int>, IMediaItem
    {
        public TimeSpan Duration { get; set; }

        public PrivacyStatus PrivacyStatus { get; set; }
        public MediaItemType MediaItemType { get; set; }

        public int PlaylistId { get; set; }

        public virtual PlaylistModel Playlist { get; set; }
    }
}
