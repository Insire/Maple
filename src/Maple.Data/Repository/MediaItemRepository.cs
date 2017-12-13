using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Maple.Domain;

namespace Maple.Data
{
    public class MediaItemRepository : MaplePlaylistRepository<MediaItemModel>, IMediaItemRepository
    {
        public Task<MediaItemModel> GetMediaItemByPlaylistIdAsync(int id)
        {
            return Task.Run(() =>
            {
                using (var context = new PlaylistContext())
                    return GetMediaItemByPlaylistIdInternalAsync(id, context);
            });
        }

        protected override DbSet<MediaItemModel> GetEntities(PlaylistContext context)
        {
            return context.MediaItems;
        }

        private MediaItemModel GetMediaItemByPlaylistIdInternalAsync(int id, PlaylistContext context)
        {
            return context.MediaItems.FirstOrDefault(p => p.Playlist.Id == id);
        }
    }
}
