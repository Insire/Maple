using System.Collections.Generic;
using System.Threading.Tasks;
using Maple.Domain;

namespace Maple.Data
{
    public class MediaPlayerRepository : IMediaPlayerRepository
    {
        public Task<IReadOnlyCollection<MediaPlayerModel>> GetAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<MediaPlayerModel> GetByIdAsync(int Id)
        {
            throw new System.NotImplementedException();
        }

        public Task<MediaPlayerModel> GetMainMediaPlayerAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyCollection<MediaPlayerModel>> GetOptionalMediaPlayersAsync()
        {
            throw new System.NotImplementedException();
        }

        public void Save(MediaPlayerModel item)
        {
            throw new System.NotImplementedException();
        }
    }
}
