using System.Threading.Tasks;

namespace Maple.Core
{
    public interface ILoadableViewModel
    {
        /// <summary>
        /// Gets a value indicating whether this instance is loaded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is loaded; otherwise, <c>false</c>.
        /// </value>
        bool IsLoaded { get; }

        IAsyncCommand LoadCommand { get; }
        IAsyncCommand RefreshCommand { get; }

        Task LoadAsync();
    }
}
