using System.ComponentModel.DataAnnotations.Schema;

namespace Maple.Data
{
    public class MediaPlayer : BaseObject
    {
        [ForeignKey(nameof(PlaylistId))]
        public Playlist Playlist { get; set; }
        public int PlaylistId { get; set; }

        public string Name { get; set; }
        public string DeviceName { get; set; }
        public bool IsPrimary { get; set; }
    }
}
