using System.Collections.Generic;
using System.Diagnostics;

namespace Maple.Domain
{
    [DebuggerDisplay("{Title}, {SelectedItem}, {Sequence}")]
    public class PlaylistModel : BaseObject<int>
    {
        private ICollection<MediaItemModel> _mediaItems;
        public virtual ICollection<MediaItemModel> MediaItems
        {
            get { return _mediaItems ?? (_mediaItems = new HashSet<MediaItemModel>()); }
            set { _mediaItems = value; }
        }

        public string Description { get; set; }

        public string Location { get; set; }
        public bool IsShuffeling { get; set; }
        public int PrivacyStatus { get; set; }
        public int RepeatMode { get; set; }
        public int? MediaPlayerId { get; set; }
        public virtual MediaPlayerModel MediaPlayer { get; set; }
        public string Title { get; set; }
    }
}
