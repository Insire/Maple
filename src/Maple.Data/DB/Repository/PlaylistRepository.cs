using System.Collections.Generic;
using System.Threading;
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

        public override Task<ICollection<PlaylistModel>> ReadAsync(CancellationToken token)
        {
            return ReadAsync(null, new[] { nameof(PlaylistModel.MediaItems) }, -1, -1, token);
        }
    }
}
