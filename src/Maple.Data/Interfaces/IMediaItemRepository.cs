using System.Threading.Tasks;
using Maple.Interfaces;

namespace Maple.Data
{
    public interface IMediaItemRepository : IMapleRepository<MediaItem>
    {
        Task<MediaItem> GetMediaItemByPlaylistIdAsync(int id);
    }
}