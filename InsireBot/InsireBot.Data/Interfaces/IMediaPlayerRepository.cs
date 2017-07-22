using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maple.Data
{

    public interface IMediaPlayerRepository : IMapleRepository<MediaPlayer>
    {
        Task<MediaPlayer> GetMainMediaPlayerAsync();
        Task<List<MediaPlayer>> GetOptionalMediaPlayersAsync();
    }
}