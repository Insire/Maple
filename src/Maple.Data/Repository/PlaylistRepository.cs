using System.Collections.Generic;
using System.Threading.Tasks;
using Maple.Domain;

namespace Maple.Data
{
    public class PlaylistRepository : IPlaylistRepository
    {
        public Task<IReadOnlyCollection<PlaylistModel>> GetAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<PlaylistModel> GetByIdAsync(int Id)
        {
            throw new System.NotImplementedException();
        }

        public void Save(PlaylistModel item)
        {
            throw new System.NotImplementedException();
        }
    }
}
