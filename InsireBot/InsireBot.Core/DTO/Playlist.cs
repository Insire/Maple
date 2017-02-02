using System.Collections.Generic;

namespace Maple.Core
{
    public class Playlist
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int RepeatMode { get; set; }
        public bool IsShuffeling { get; set; }
        public bool IsRestricted { get; set; }
        public List<MediaItem> MediaItems { get; set; }
    }
}
