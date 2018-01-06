using System.Data.Entity;
using System.Threading.Tasks;
using Maple.Domain;

namespace Maple.Data
{
    public class MediaItemRepository : MaplePlaylistRepository<MediaItemModel, int>, IMediaItemRepository
    {
        public async Task<MediaItemModel> GetMediaItemByPlaylistIdAsync(int id)
        {
            using (var context = new PlaylistContext())
                return await GetMediaItemByPlaylistIdInternalAsync(id, context).ConfigureAwait(false);
        }

        protected override DbSet<MediaItemModel> GetEntities(PlaylistContext context)
        {
            return context.MediaItems;
        }

        private async Task<MediaItemModel> GetMediaItemByPlaylistIdInternalAsync(int id, PlaylistContext context)
        {
            return await GetEntities(context).FirstOrDefaultAsync(p => p.Playlist.Id == id).ConfigureAwait(false);
        }
    }
}
