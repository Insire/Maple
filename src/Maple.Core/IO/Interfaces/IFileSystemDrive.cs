namespace Maple.Core
{
    public interface IFileSystemDrive : IFileSystemInfo
    {
        RangeObservableCollection<IFileSystemInfo> Children { get; }
    }
}
