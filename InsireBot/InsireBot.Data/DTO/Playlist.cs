using System.Collections.Generic;

namespace Maple.Data
{
    public class Playlist : BaseObject
    {
        public Playlist()
        {
            MediaItems = new List<MediaItem>();
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int RepeatMode { get; set; }
        public bool IsShuffeling { get; set; }
        public int PrivacyStatus { get; set; }
        public ICollection<MediaItem> MediaItems { get; set; }
    }
}
