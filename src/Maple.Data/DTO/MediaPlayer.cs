using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Maple.Data
{
    [DebuggerDisplay("{Name}, {DeviceName}, {Sequence}")]
    public class MediaPlayer : BaseObject
    {
        public int PlaylistId { get; set; }
        [ForeignKey(nameof(PlaylistId))]
        public Playlist Playlist { get; set; }

        [MaxLength(100)]
        public string DeviceName { get; set; }
        public bool IsPrimary { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
