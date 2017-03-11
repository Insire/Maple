using System.Collections.Generic;

namespace Maple.Data
{
    public class Playlist : BaseObject
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int RepeatMode { get; set; }
        public bool IsShuffeling { get; set; }
        public int PrivacyStatus { get; set; }
        public List<MediaItem> MediaItems { get; set; } = new List<MediaItem>();
    }
}
