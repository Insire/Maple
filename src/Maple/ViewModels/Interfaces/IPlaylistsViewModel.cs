using System.Windows.Input;
using Maple.Core;

namespace Maple
{
    public interface IPlaylistsViewModel : ILoadableViewModel, ISaveableViewModel
    {
        ICommand PlayCommand { get; }

        void Add();
    }
}