using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Maple.Data
{
    public class MediaItemRepository : MaplePlaylistRepository<MediaItem>, IMediaItemRepository
    {
        public Task<MediaItem> GetMediaItemByPlaylistIdAsync(int id)
        {
            return Task.Run(() =>
            {
                using (var context = new PlaylistContext())
                    return GetMediaItemByPlaylistIdInternalAsync(id, context);
            });
        }

        protected override DbSet<MediaItem> GetEntities(PlaylistContext context)
        {
            return context.MediaItems;
        }

        private MediaItem GetMediaItemByPlaylistIdInternalAsync(int id, PlaylistContext context)
        {
            return context.MediaItems.FirstOrDefault(p => p.Playlist.Id == id);
        }
    }
}
