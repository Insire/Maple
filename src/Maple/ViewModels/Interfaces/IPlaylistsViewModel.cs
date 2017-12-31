using Maple.Core;

namespace Maple
{
    public interface IPlaylistsViewModel : ILoadableViewModel, ISaveableViewModel
    {
        void Add();
    }
}