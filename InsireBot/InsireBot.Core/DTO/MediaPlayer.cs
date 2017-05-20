using System;

namespace Maple.Core
{
    public class MediaPlayer
    {
        public int Id { get; set; }
        public int Sequence { get; set; }

        public Playlist Playlist { get; set; }
        public int PlaylistId { get; set; }

        public string Name { get; set; }
        public string DeviceName { get; set; }
        public bool IsPrimary { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
