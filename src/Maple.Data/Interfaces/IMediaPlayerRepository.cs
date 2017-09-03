using System.Collections.Generic;
using System.Threading.Tasks;
using Maple.Interfaces;

namespace Maple.Data
{
    public interface IMediaPlayerRepository : IMapleRepository<MediaPlayer>
    {
        Task<MediaPlayer> GetMainMediaPlayerAsync();
        Task<IReadOnlyCollection<MediaPlayer>> GetOptionalMediaPlayersAsync();
    }
}