using System.Collections.Generic;

namespace Maple.Youtube
{
    public sealed class YoutubePlaylist : YoutubeRessource
    {
        public ICollection<YoutubeVideo> Items { get; set; }
    }
}
