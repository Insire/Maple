using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Maple.Domain;
using Microsoft.EntityFrameworkCore;

namespace Maple.Data
{
    public sealed class MediaPlayerRepository : Repository<MediaPlayerModel, int>, IMediaPlayerRepository
    {
        public MediaPlayerRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public override async Task<ICollection<MediaPlayerModel>> ReadAsync(CancellationToken token)
        {
            var result = await ReadInternal(null, null, -1, -1)
                .Include(p => p.Playlist)
                .ThenInclude(p => p.MediaItems)
                .ToListAsync(token)
                .ConfigureAwait(false);

            return result.AsReadOnly();
        }

        public async Task<MediaPlayerModel> GetMainMediaPlayerAsync(CancellationToken token)
        {
            var result = await ReadAsync(p => p.IsPrimary, new[] { nameof(MediaPlayerModel.Playlist) }, -1, 1, token).ConfigureAwait(false);
            return result.FirstOrDefault();
        }

        public Task<ICollection<MediaPlayerModel>> GetOptionalMediaPlayersAsync(CancellationToken token)
        {
            return ReadAsync(p => !p.IsPrimary, new[]
            {
                nameof(MediaPlayerModel.Playlist)
                ,$"{nameof(MediaPlayerModel.Playlist)}.{nameof(PlaylistModel.MediaItems)}"
            }, -1, -1, token);
        }
    }
}
