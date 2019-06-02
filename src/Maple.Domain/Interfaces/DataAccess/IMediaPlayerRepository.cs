using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Maple.Domain
{
    public interface IMediaPlayerRepository : IRepository<MediaPlayerModel, int>
    {
        Task<MediaPlayerModel> GetMainMediaPlayerAsync(CancellationToken token);

        Task<ICollection<MediaPlayerModel>> GetOptionalMediaPlayersAsync(CancellationToken token);
    }
}
