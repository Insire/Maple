namespace Maple.Core
{
    public interface ILoadableViewModel
    {
        bool IsLoaded { get; }

        IAsyncCommand LoadCommand { get; }
        IAsyncCommand RefreshCommand { get; }
    }
}
