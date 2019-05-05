using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Maple.Domain;

namespace Maple.Data
{
    public sealed class MediaPlayerRepository : Repository<MediaPlayerModel, int>, IMediaPlayerRepository
    {
        public MediaPlayerRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public async Task<MediaPlayerModel> GetMainMediaPlayerAsync()
        {
            var result = await ReadAsync(p => p.IsPrimary, new[] { nameof(MediaPlayerModel.Playlist) }, -1, 1).ConfigureAwait(false);
            return result.FirstOrDefault();
        }

        public Task<ICollection<MediaPlayerModel>> GetOptionalMediaPlayersAsync()
        {
            return ReadAsync(p => !p.IsPrimary, new[] { nameof(MediaPlayerModel.Playlist) }, -1, -1);
        }
    }
}
