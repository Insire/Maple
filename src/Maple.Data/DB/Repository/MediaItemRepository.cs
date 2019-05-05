using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Maple.Domain;

namespace Maple.Data
{
    public sealed class MediaItemRepository : Repository<MediaItemModel, int>, IMediaItemRepository
    {
        public MediaItemRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public async Task<MediaItemModel> GetMediaItemByPlaylistIdAsync(int id)
        {
            var result = await ReadAsync(p => p.PlaylistId.Equals(id), new[] { nameof(MediaItemModel.Playlist) }, -1, 1).ConfigureAwait(false);

            return result.First();
        }

        public Task<ICollection<MediaItemModel>> ReadAsync()
        {
            return ReadAsync(null, new[] { nameof(MediaItemModel.Playlist) }, -1, -1);
        }
    }
}
