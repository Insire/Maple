using System.Threading;
using System.Threading.Tasks;

namespace Maple.Domain
{
    public interface IMediaItemRepository : IRepository<MediaItemModel, int>
    {
        Task<MediaItemModel> GetMediaItemByPlaylistIdAsync(int id, CancellationToken token);
    }
}
