using Maple.Interfaces;

namespace Maple.Core
{
    public interface IFileSystemDrive : IFileSystemInfo
    {
        IRangeObservableCollection<IFileSystemInfo> Children { get; }
    }
}
