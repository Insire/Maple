using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Maple.Domain
{
    [DebuggerDisplay("{Name}, {DeviceName}, {Sequence}")]
    public class MediaPlayerModel : BaseModel<int>
    {
        public int PlaylistId { get; set; }
        [ForeignKey(nameof(PlaylistId))]
        public PlaylistModel Playlist { get; set; }

        [MaxLength(100)]
        public string DeviceName { get; set; }
        public bool IsPrimary { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
