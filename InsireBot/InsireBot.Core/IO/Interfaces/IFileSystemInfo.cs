using System.ComponentModel;
using System.Windows.Input;

namespace Maple.Core
{
    public interface IFileSystemInfo : INotifyPropertyChanged
    {
        /// <summary>
        /// conditional refresh
        /// </summary>
        ICommand LoadCommand { get; }
        /// <summary>
        /// explicit refresh
        /// </summary>
        ICommand RefreshCommand { get; }
        /// <summary>
        /// delete an object from the filesystem
        /// </summary>
        ICommand DeleteCommand { get; }
        /// <summary>
        /// the logical parent
        /// </summary>
        IFileSystemDirectory Parent { get; }

        string Name { get; }
        string FullName { get; }
        string Filter { get; set; }

        bool IsExpanded { get; set; }
        bool IsSelected { get; set; }

        bool Exists { get; }
        bool IsLoaded { get; }
        bool IsBusy { get; }
        bool IsHidden { get; }
        bool IsContainer { get; }

        bool HasContainers { get; }

        /// <summary>
        /// updates filtering and the current children
        /// </summary>
        void Refresh();
        /// <summary>
        /// Loads attributes and checks if the object still exists on file system
        /// </summary>
        void LoadMetaData();
        /// <summary>
        /// updates the filtering
        /// </summary>
        /// <param name="filter"></param>
        void OnFilterChanged(string filter);

        void Delete();
        bool CanDelete();
    }
}
