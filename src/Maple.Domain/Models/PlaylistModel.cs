using System.Collections.Generic;
using System.Diagnostics;

namespace Maple.Domain
{
    [DebuggerDisplay("PlaylistModel: {Sequence}, {Name}")]
    public class PlaylistModel : Entity<int>, IPlaylist
    {
        public bool IsShuffeling { get; set; }
        public PrivacyStatus PrivacyStatus { get; set; }
        public RepeatMode RepeatMode { get; set; }

        public virtual List<MediaPlayerModel> MediaPlayers { get; set; }

        public virtual List<MediaItemModel> MediaItems { get; set; }

        public PlaylistModel()
        {
            MediaItems = new List<MediaItemModel>();
            MediaPlayers = new List<MediaPlayerModel>();
        }
    }
}
