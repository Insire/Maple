using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Maple.Domain;

namespace Maple.Data
{
    public class MediaPlayerRepository : MaplePlaylistRepository<MediaPlayerModel>, IMediaPlayerRepository
    {
        public Task<MediaPlayerModel> GetMainMediaPlayerAsync()
        {
            return Task.Run(() =>
             {
                 using (var context = new PlaylistContext())
                     return GetMainMediaPlayerInternal(context);
             });
        }

        private MediaPlayerModel GetMainMediaPlayerInternal(PlaylistContext context)
        {
            return GetEntities(context).Include(p => p.Playlist)
                                        .Include(p => p.Playlist.MediaItems)
                                        .FirstOrDefault(p => p.IsPrimary);
        }

        public Task<IReadOnlyCollection<MediaPlayerModel>> GetOptionalMediaPlayersAsync()
        {
            return Task.Run(() =>
            {
                using (var context = new PlaylistContext())
                    return GetOptionalMediaPlayersInternal(context);
            });
        }

        private IReadOnlyCollection<MediaPlayerModel> GetOptionalMediaPlayersInternal(PlaylistContext context)
        {
            return GetEntities(context).Include(p => p.Playlist)
                                            .Where(p => !p.IsPrimary)
                                            .ToList();
        }

        protected override IReadOnlyCollection<MediaPlayerModel> GetInternalAsync(PlaylistContext context)
        {
            return GetEntities(context).Include(p => p.Playlist).ToList();
        }

        protected override DbSet<MediaPlayerModel> GetEntities(PlaylistContext context)
        {
            return context.Mediaplayers;
        }
    }
}
