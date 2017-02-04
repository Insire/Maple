using System.Collections.Generic;

namespace Maple.Data
{
    public interface IMediaItemRepository : IRepository<MediaItem>
    {
        List<MediaItem> GetAllByPlaylistId(int id);
    }
}
