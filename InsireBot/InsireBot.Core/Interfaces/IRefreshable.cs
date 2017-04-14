using System.Threading.Tasks;

namespace Maple.Core
{
    /// <summary>
    ///
    /// </summary>
    public interface IRefreshable
    {
        /// <summary>
        /// Saves this instance.
        /// </summary>
        void Save();
        /// <summary>
        /// Loads this instance.
        /// </summary>
        void Load();

        /// <summary>
        /// Saves this instance.
        /// </summary>
        Task SaveAsync();
        /// <summary>
        /// Loads this instance.
        /// </summary>
        Task LoadAsync();
    }
}
