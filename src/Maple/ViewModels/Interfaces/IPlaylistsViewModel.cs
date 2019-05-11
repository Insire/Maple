using System.Threading.Tasks;
using Maple.Domain;

namespace Maple
{
    public interface IPlaylistsViewModel : IBaseListViewModel<Playlist>
    {
        Task Add();
    }
}
