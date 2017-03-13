using System.Windows.Input;

namespace Maple.Core
{
    public interface ILoadableViewModel : IRefreshable
    {
        bool IsLoaded { get; }
        ICommand LoadCommand { get; }
        ICommand RefreshCommand { get; }
    }
}
