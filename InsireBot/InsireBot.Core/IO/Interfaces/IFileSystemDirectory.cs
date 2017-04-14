namespace Maple.Core
{
    public interface IFileSystemDirectory : IFileSystemInfo
    {
        RangeObservableCollection<IFileSystemInfo> Children { get; }
    }
}
