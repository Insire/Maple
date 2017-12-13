using System.Collections.Generic;
using System.Threading.Tasks;
using Maple.Domain;

namespace Maple.Data
{
    public interface IMediaPlayerRepository : IMapleRepository<MediaPlayerModel>
    {
        Task<MediaPlayerModel> GetMainMediaPlayerAsync();
        Task<IReadOnlyCollection<MediaPlayerModel>> GetOptionalMediaPlayersAsync();
    }
}