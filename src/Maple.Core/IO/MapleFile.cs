using System.Diagnostics;
using System.IO;

namespace Maple.Core
{
    [DebuggerDisplay("File: {Name} IsContainer: {IsContainer}")]
    public class MapleFile : MapleFileSystemBase, IFileSystemFile
    {
        public MapleFile(FileInfo info, IDepth depth, IFileSystemDirectory parent, IMessenger messenger)
            : base(info.Name, info.FullName, depth, parent, messenger)
        {
            using (BusyStack.GetToken())
            {
                if (!Depth.IsMaxReached)
                {
                    Refresh();
                    IsExpanded = true;
                }
            }
        }

        public override void Refresh()
        {
            using (BusyStack.GetToken())
            {
                OnFilterChanged(string.Empty);
            }
        }

        public override void OnFilterChanged(string filter)
        {
            Filter = filter;
        }

        public override void LoadMetaData()
        {
            var info = new FileInfo(FullName);

            Exists = info.Exists;
            IsHidden = info.Attributes.HasFlag(FileAttributes.Hidden);
        }

        public override void Delete()
        {
            File.Delete(FullName);
            Parent.Refresh();
        }

        public override bool CanDelete()
        {
            return File.Exists(FullName);
        }
    }
}
