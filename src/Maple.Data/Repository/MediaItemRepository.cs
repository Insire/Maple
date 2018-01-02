using System.Threading.Tasks;
using Maple.Domain;

namespace Maple.Data
{
    public class MediaItemRepository : BaseRepository<MediaItemModel>, IMediaItemRepository
    {
        public MediaItemRepository(IConnectionStringManager connectionStringManager)
            : base(connectionStringManager)
        {
        }

        public Task<MediaItemModel> GetMediaItemByPlaylistIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
