using Maple.Core;
using Maple.Localization.Properties;
using System.Collections.Generic;

namespace Maple.Data
{
    public class Playlist : DatabaseObject
    {
        public static Playlist New()
        {
            return new Playlist
            {
                Title = Resources.New,
                Description = string.Empty,
                Location = string.Empty,
                MediaItems = new List<MediaItem>(),
                RepeatMode = 0,
                IsShuffeling = false,
                Sequence = 0,
            };
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int RepeatMode { get; set; }
        public bool IsShuffeling { get; set; }
        public int PrivacyStatus { get; set; }
        public List<MediaItem> MediaItems { get; set; } = new List<MediaItem>();
    }
}
