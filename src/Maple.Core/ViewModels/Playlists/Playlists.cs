using Maple.Domain;

namespace Maple.Core
{
    public class Playlists : ValidableBaseDataListViewModel<Playlist, PlaylistModel, int>, IPlaylistsViewModel
    {
        protected Playlists(ViewModelServiceContainer container)
            : base(container)
        {
        }
    }
}
