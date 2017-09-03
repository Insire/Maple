using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Maple.Data
{
    public class MediaPlayerRepository : MaplePlaylistRepository<MediaPlayer>, IMediaPlayerRepository
    {
        public Task<MediaPlayer> GetMainMediaPlayerAsync()
        {
            return Task.Run(() =>
             {
                 using (var context = new PlaylistContext())
                     return GetMainMediaPlayerInternal(context);
             });
        }

        private MediaPlayer GetMainMediaPlayerInternal(PlaylistContext context)
        {
            return GetEntities(context).Include(p => p.Playlist)
                             .FirstOrDefault(p => p.IsPrimary);
        }

        public Task<IReadOnlyCollection<MediaPlayer>> GetOptionalMediaPlayersAsync()
        {
            return Task.Run(() =>
            {
                using (var context = new PlaylistContext())
                    return GetOptionalMediaPlayersInternal(context);
            });
        }

        private IReadOnlyCollection<MediaPlayer> GetOptionalMediaPlayersInternal(PlaylistContext context)
        {
            return GetEntities(context).Include(p => p.Playlist)
                                            .Where(p => !p.IsPrimary)
                                            .ToList();
        }

        protected override IReadOnlyCollection<MediaPlayer> GetInternalAsync(PlaylistContext context)
        {
            return GetEntities(context).Include(p => p.Playlist).ToList();
        }

        protected override DbSet<MediaPlayer> GetEntities(PlaylistContext context)
        {
            return context.Mediaplayers;
        }
    }
}
