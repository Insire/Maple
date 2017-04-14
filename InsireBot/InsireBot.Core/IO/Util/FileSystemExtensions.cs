using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Permissions;

namespace Maple.Core
{
    public static class FileSystemExtensions
    {
        public static List<IFileSystemInfo> GetChildren(this MapleFileSystemContainerBase directory, IDepth depth)
        {
            var result = new List<IFileSystemInfo>();

            if (!CanAccess(directory.FullName) && directory.DirectoryIsEmpty())
                return result;

            result.AddRange(GetDirectories(directory.FullName, depth, directory));
            result.AddRange(GetFiles(directory.FullName, depth, directory));

            return result;
        }

        private static bool CanAccess(string path)
        {
            var permission = new FileIOPermission(FileIOPermissionAccess.AllAccess, AccessControlActions.View, path);

            try
            {
                permission.Demand();
                return true;
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.WriteLine($"{nameof(UnauthorizedAccessException)} occured during reading off {path}");
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        private static List<IFileSystemInfo> GetDirectories(string path, IDepth depth, IFileSystemDirectory parent)
        {
            var result = new List<IFileSystemInfo>();
            try
            {
                var directories = Directory.GetDirectories(path)
                                            .Select(p => new DirectoryInfo(p))
                                            .Where(p => p.Attributes.HasFlag(FileAttributes.Directory)
                                                        && !p.Attributes.HasFlag(FileAttributes.Hidden)
                                                        && !p.Attributes.HasFlag(FileAttributes.System)
                                                        && !p.Attributes.HasFlag(FileAttributes.Offline)
                                                        && !p.Attributes.HasFlag(FileAttributes.Encrypted))
                                            .Select(p => new MapleDirectory(p, depth, parent))
                                            .ToList();

                result.AddRange(directories);
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.WriteLine($"{nameof(UnauthorizedAccessException)} occured during reading off {path}");
                Debug.WriteLine(ex.Message);
            }
            return result;
        }

        private static List<IFileSystemInfo> GetFiles(string path, IDepth depth, IFileSystemDirectory parent)
        {
            var result = new List<IFileSystemInfo>();
            try
            {
                var files = Directory.GetFiles(path)
                                        .Select(p => new FileInfo(p))
                                        .Where(p => !p.Attributes.HasFlag(FileAttributes.Directory)
                                                    && !p.Attributes.HasFlag(FileAttributes.Hidden)
                                                    && !p.Attributes.HasFlag(FileAttributes.System)
                                                    && !p.Attributes.HasFlag(FileAttributes.Offline)
                                                    && !p.Attributes.HasFlag(FileAttributes.Encrypted))
                                        .Select(p => new MapleFile(p, depth, parent))
                                        .ToList();

                result.AddRange(files);
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.WriteLine($"{nameof(UnauthorizedAccessException)} occured during reading off {path}");
                Debug.WriteLine(ex.Message);
            }
            return result;
        }

        public static bool DirectoryIsEmpty(this MapleFileSystemContainerBase info)
        {
            if (!Directory.Exists(info.FullName))
                return false;

            return DirectoryIsEmpty(info.FullName);
        }

        public static bool DirectoryIsEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        public static void ExpandPath(this IFileSystemInfo item)
        {
            item.IsExpanded = true;
            var parent = item.Parent;

            while (parent != null)
            {
                parent.IsExpanded = true;
                parent = parent.Parent;
            }
        }
    }
}
