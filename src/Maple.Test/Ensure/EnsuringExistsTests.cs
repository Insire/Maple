using System.IO;
using Maple.Core;
using Maple.Localization.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Maple.Test
{
    [TestClass]
    public sealed class EnsuringExistsTests
    {
        [TestMethod]
        public void When_ensuring_directoryInfo_exists()
        {
            var nonExistingDirectoryPath = Path.GetRandomFileName();
            var nonExistingDirInfo = new DirectoryInfo(nonExistingDirectoryPath);

            Should.Throw<DirectoryNotFoundException>(() => Ensure.Exists(nonExistingDirInfo)).Message.ShouldBe($"{Resources.ExceptionMessageMissingDirectory}{nonExistingDirInfo.FullName}");

            nonExistingDirInfo.Create();

            var existingDirInfo = nonExistingDirInfo;

            Should.NotThrow(() => Ensure.Exists(existingDirInfo));
            Ensure.Exists(existingDirInfo).ShouldBe(existingDirInfo);

            try
            {
                existingDirInfo.Delete();
            }
            catch { /* ignored */ }
        }

        [TestMethod]
        public void When_ensuring_fileInfo_exists()
        {
            var nonExistingFilePath = Path.GetRandomFileName();
            var nonExistingFileInfo = new FileInfo(nonExistingFilePath);

            Should.Throw<FileNotFoundException>(() => Ensure.Exists(nonExistingFileInfo)).Message.ShouldBe($"{Resources.ExceptionMessageMissingFile}{nonExistingFileInfo.FullName}");

            nonExistingFileInfo.Create();

            var existingFileInfo = nonExistingFileInfo;

            Should.NotThrow(() => Ensure.Exists(existingFileInfo));
            Ensure.Exists(existingFileInfo).ShouldBe(existingFileInfo);

            try
            {
                existingFileInfo.Delete();
            }
            catch { /* ignored */ }
        }
    }
}
