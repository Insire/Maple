using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Maple.Domain;

namespace Maple.Data
{
    public class PlaylistRepository : MaplePlaylistRepository<PlaylistModel, int>, IPlaylistRepository
    {
        protected override DbSet<PlaylistModel> GetEntities(PlaylistContext context)
        {
            return context.Playlists;
        }

        protected override async Task<List<PlaylistModel>> GetInternalAsync(PlaylistContext context)
        {
            return await GetEntities(context).Include(p => p.MediaItems).ToListAsync().ConfigureAwait(false);
        }
    }
}
