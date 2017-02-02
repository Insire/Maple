using System.Collections.Generic;

namespace Maple.Data
{
    public interface IMediaPlayerRepository : IRepository<MediaPlayer>
    {
        IEnumerable<MediaPlayer> GetPrimary();
    }
}
