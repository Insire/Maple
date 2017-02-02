using System;

namespace Maple.Core
{
    public class MediaItem
    {
        public string Title { get; set; }
        public long Duration { get; set; }

        public string Location { get; set; }
        public bool IsRestricted { get; set; }
    }
}
