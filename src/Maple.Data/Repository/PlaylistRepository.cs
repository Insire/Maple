using Maple.Domain;

namespace Maple.Data
{
    public class PlaylistRepository : BaseRepository<PlaylistModel>, IPlaylistRepository
    {
        public PlaylistRepository(IConnectionStringManager connectionStringManager)
            : base(connectionStringManager)
        {
        }
    }
}
