using System.Collections.Generic;

namespace InsireBot.Data
{
    public interface IMediaPlayerRepository : IRepository<MediaPlayer>
    {
        IEnumerable<MediaPlayer> GetPrimary();
    }
}
