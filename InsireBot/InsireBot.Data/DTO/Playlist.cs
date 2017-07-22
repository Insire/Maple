using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Maple.Data
{
    // tutorial http://www.entityframeworktutorial.net/code-first/configure-one-to-many-relationship-in-code-first.aspx

    [DebuggerDisplay("{Title}, {SelectedItem}, {Sequence}")]
    public class Playlist : BaseObject
    {
        private ICollection<MediaItem> _mediaItems;
        public virtual ICollection<MediaItem> MediaItems
        {
            get { return _mediaItems ?? (_mediaItems = new HashSet<MediaItem>()); }
            set { _mediaItems = value; }
        }

        [MaxLength(100)]
        public string Description { get; set; }
        public string Location { get; set; }
        public bool IsShuffeling { get; set; }
        public int PrivacyStatus { get; set; }
        public int RepeatMode { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

    }
}
