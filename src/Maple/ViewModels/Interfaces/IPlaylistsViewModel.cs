using System.Threading.Tasks;
using Maple.Core;
using Maple.Domain;

namespace Maple
{
    public interface IPlaylistsViewModel : ILoadableViewModel, ISaveableViewModel, IBaseListViewModel<Playlist>
    {
        Task Add();
    }
}
