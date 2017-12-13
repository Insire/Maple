using Maple.Domain;

namespace Maple.Core
{
    public interface IFileSystemDirectory : IFileSystemInfo
    {
        IRangeObservableCollection<IFileSystemInfo> Children { get; }
    }
}
