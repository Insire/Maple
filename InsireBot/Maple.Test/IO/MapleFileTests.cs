using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;

namespace Maple.Test.IO
{
    [TestClass]
    public class MapleFileTests
    {
        private MockRepository _mockRepository;

        private Mock<FileInfo> _mockFileInfo;
        private Mock<IDepth> _mockDepth;
        private Mock<IFileSystemDirectory> _mockFileSystemDirectory;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockFileInfo = _mockRepository.Create<FileInfo>();
            _mockDepth = _mockRepository.Create<IDepth>();
            _mockFileSystemDirectory = _mockRepository.Create<IFileSystemDirectory>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mockRepository.VerifyAll();
        }

        [TestMethod]
        public void MapleFileCtorTest()
        {
            var mapleFile = CreateMapleFile();
        }

        private MapleFile CreateMapleFile()
        {
            return new MapleFile(
                _mockFileInfo.Object,
                _mockDepth.Object,
                _mockFileSystemDirectory.Object);
        }
    }
}