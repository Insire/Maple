using System.Collections.Generic;
using System.Threading.Tasks;
using Maple.Domain;

namespace Maple.Data
{
    public sealed class PlaylistRepository : Repository<PlaylistModel, int>, IPlaylistRepository
    {
        public PlaylistRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public Task<ICollection<PlaylistModel>> ReadAsync()
        {
            return ReadAsync(null, new[] { nameof(PlaylistModel.MediaItems) }, -1, -1);
        }
    }
}
