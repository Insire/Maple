using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;

namespace Maple.Test.IO
{
    [TestClass]
    public class MapleDirectoryTests
    {
        private MockRepository _mockRepository;

        private Mock<DirectoryInfo> _mockDirectoryInfo;
        private Mock<IDepth> _mockDepth;
        private Mock<IFileSystemDirectory> _mockFileSystemDirectory;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockDirectoryInfo = _mockRepository.Create<DirectoryInfo>();
            _mockDepth = _mockRepository.Create<IDepth>();
            _mockFileSystemDirectory = _mockRepository.Create<IFileSystemDirectory>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mockRepository.VerifyAll();
        }

        [TestMethod]
        public void MapleDirectoryCtorTest()
        {
            var mapleDirectory = CreateMapleDirectory();
        }

        private MapleDirectory CreateMapleDirectory()
        {
            return new MapleDirectory(
                _mockDirectoryInfo.Object,
                _mockDepth.Object,
                _mockFileSystemDirectory.Object);
        }
    }
}