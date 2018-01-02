using System.Collections.Generic;
using System.Threading.Tasks;
using Maple.Domain;

namespace Maple.Data
{
    public class MediaPlayerRepository : BaseRepository<MediaPlayerModel>, IMediaPlayerRepository
    {
        public MediaPlayerRepository(IConnectionStringManager connectionStringManager)
            : base(connectionStringManager)
        {
        }

        public Task<MediaPlayerModel> GetMainMediaPlayerAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyCollection<MediaPlayerModel>> GetOptionalMediaPlayersAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
