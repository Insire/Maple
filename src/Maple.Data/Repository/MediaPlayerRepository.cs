using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Maple.Domain;

namespace Maple.Data
{
    public class MediaPlayerRepository : MaplePlaylistRepository<MediaPlayerModel, int>, IMediaPlayerRepository
    {
        public async Task<MediaPlayerModel> GetMainMediaPlayerAsync()
        {
            using (var context = new PlaylistContext())
                return await GetMainMediaPlayerInternal(context).ConfigureAwait(false);
        }

        private async Task<MediaPlayerModel> GetMainMediaPlayerInternal(PlaylistContext context)
        {
            return await GetEntities(context).Include(p => p.Playlist)
                                        .Include(p => p.Playlist.MediaItems)
                                        .FirstOrDefaultAsync(p => p.IsPrimary)
                                        .ConfigureAwait(false);
        }

        public async Task<IReadOnlyCollection<MediaPlayerModel>> GetOptionalMediaPlayersAsync()
        {
            using (var context = new PlaylistContext())
                return await GetOptionalMediaPlayersInternal(context).ConfigureAwait(false);
        }

        private async Task<IReadOnlyCollection<MediaPlayerModel>> GetOptionalMediaPlayersInternal(PlaylistContext context)
        {
            return await GetEntities(context).Include(p => p.Playlist)
                                            .Where(p => !p.IsPrimary)
                                            .ToListAsync()
                                            .ConfigureAwait(false);
        }

        protected override async Task<List<MediaPlayerModel>> GetInternalAsync(PlaylistContext context)
        {
            return await GetEntities(context).Include(p => p.Playlist).ToListAsync().ConfigureAwait(false);
        }

        protected override DbSet<MediaPlayerModel> GetEntities(PlaylistContext context)
        {
            return context.Mediaplayers;
        }
    }
}
