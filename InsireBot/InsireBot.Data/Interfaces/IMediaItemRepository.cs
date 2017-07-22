using System.Threading.Tasks;

namespace Maple.Data
{
    public interface IMediaItemRepository : IMapleRepository<MediaItem>
    {
        Task<MediaItem> GetMediaItemByPlaylistIdAsync(int id);
    }
}