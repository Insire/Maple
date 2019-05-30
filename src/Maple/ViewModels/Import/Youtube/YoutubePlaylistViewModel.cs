using System.Collections.ObjectModel;
using System.Linq;
using Maple.Youtube;

namespace Maple
{
    public sealed class YoutubePlaylistViewModel : YoutubeRessourceViewModel
    {
        public ReadOnlyCollection<YoutubeVideoViewModel> Items { get; }

        public YoutubePlaylistViewModel(YoutubePlaylist model)
            : base(model)
        {
            Items = model.Items.Select(p => new YoutubeVideoViewModel(p)).ToList().AsReadOnly();
        }
    }
}
