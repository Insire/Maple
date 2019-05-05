using System.Diagnostics;

namespace Maple.Domain
{
    [DebuggerDisplay("{Name}, {DeviceName}, {Sequence}")]
    public class MediaPlayerModel : BaseObject<int>
    {
        public int? PlaylistId { get; set; }

        public virtual PlaylistModel Playlist { get; set; }

        public string DeviceName { get; set; }

        public bool IsPrimary { get; set; }

        public string Name { get; set; }
    }
}
