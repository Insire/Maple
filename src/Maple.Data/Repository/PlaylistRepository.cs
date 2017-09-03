using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Maple.Data
{
    public class PlaylistRepository : MaplePlaylistRepository<Playlist>, IPlaylistRepository
    {
        protected override DbSet<Playlist> GetEntities(PlaylistContext context)
        {
            return context.Playlists;
        }

        protected override List<Playlist> GetInternalAsync(PlaylistContext context)
        {
            return GetEntities(context).Include(p => p.MediaItems).ToList();
        }
    }
}
