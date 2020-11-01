using System.Diagnostics;

namespace Maple.Domain
{
    [DebuggerDisplay("MediaPlayerModel: {Sequence}, {Name}")]
    public class MediaPlayerModel : Entity<int>, IMediaPlayer
    {
        public bool IsPrimary { get; set; }

        public int? PlaylistId { get; set; }

        public virtual PlaylistModel Playlist { get; set; }

        public int? AudioDeviceId { get; set; }

        public virtual AudioDeviceModel AudioDevice { get; set; }
    }
}
