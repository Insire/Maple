using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maple.Domain
{
    public interface IPlaylistRepository : IRepository<PlaylistModel, int>
    {
        Task<ICollection<PlaylistModel>> ReadAsync();
    }
}
