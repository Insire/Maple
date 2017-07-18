using System.Windows.Input;

namespace Maple.Core
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Maple.Core.IRefreshable" />
    public interface ILoadableViewModel : IRefreshable
    {
        /// <summary>
        /// Gets a value indicating whether this instance is loaded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is loaded; otherwise, <c>false</c>.
        /// </value>
        bool IsLoaded { get; }
        /// <summary>
        /// Gets the load command.
        /// </summary>
        /// <value>
        /// The load command.
        /// </value>
        ICommand LoadCommand { get; }
        /// <summary>
        /// Gets the refresh command.
        /// </summary>
        /// <value>
        /// The refresh command.
        /// </value>
        ICommand RefreshCommand { get; }
    }
}
