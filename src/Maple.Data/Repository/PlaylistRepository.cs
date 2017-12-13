using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Maple.Domain;

namespace Maple.Data
{
    public class PlaylistRepository : MaplePlaylistRepository<PlaylistModel>, IPlaylistRepository
    {
        protected override DbSet<PlaylistModel> GetEntities(PlaylistContext context)
        {
            return context.Playlists;
        }

        protected override IReadOnlyCollection<PlaylistModel> GetInternalAsync(PlaylistContext context)
        {
            return GetEntities(context).Include(p => p.MediaItems).ToList();
        }
    }
}
