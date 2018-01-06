using System.Threading.Tasks;
using Maple.Domain;

namespace Maple.Data
{
    public interface IMediaItemRepository : IMapleRepository<MediaItemModel, int>
    {
        Task<MediaItemModel> GetMediaItemByPlaylistIdAsync(int id);
    }
}