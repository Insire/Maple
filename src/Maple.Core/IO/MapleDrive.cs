using System.Diagnostics;
using System.IO;

using Maple.Domain;

namespace Maple.Core
{
    [DebuggerDisplay("Drive: {Name} IsContainer: {IsContainer}")]
    public class MapleDrive : MapleFileSystemContainerBase, IFileSystemDrive
    {
        public MapleDrive(DriveInfo info, IDepth depth, IMessenger messenger, ILoggingService loggingService)
            : base(info.Name, info.Name, depth, null, messenger, loggingService)
        {
            using (BusyStack.GetToken())
            {
                if (!Depth.IsMaxReached)
                    Refresh();
            }
        }

        public override void LoadMetaData()
        {
        }

        public override void Delete()
        {
        }

        public override bool CanDelete()
        {
            return false;
        }
    }
}
