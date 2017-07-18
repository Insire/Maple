namespace Maple.Core
{
    public class FileSystemInfoChangedMessage : GenericMapleMessage<IFileSystemInfo>
    {
        public FileSystemInfoChangedMessage(object sender, IFileSystemInfo info) : base(sender, info)
        {
        }
    }
}
