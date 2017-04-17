using Maple.Core;
using System.Windows.Input;

namespace Maple
{
    public interface IPlaylistsViewModel : ILoadableViewModel, ISaveableViewModel
    {
        ICommand PlayCommand { get; }

        void Add();
    }
}