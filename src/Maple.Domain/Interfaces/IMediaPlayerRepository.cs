using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maple.Domain
{
    public interface IMediaPlayerRepository : IRepository<MediaPlayerModel, int>
    {
        Task<MediaPlayerModel> GetMainMediaPlayerAsync();

        Task<ICollection<MediaPlayerModel>> GetOptionalMediaPlayersAsync();
    }
}
