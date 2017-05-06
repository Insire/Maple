using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Maple.Test.IO.Util
{
    [TestClass]
    public class FileSystemDepthTests
    {
        private MockRepository _mockRepository;



        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mockRepository.VerifyAll();
        }

        [TestMethod]
        public void FileSystemDepthCtorTest()
        {
            var fileSystemDepth = CreateFileSystemDepth(0);
        }

        private FileSystemDepth CreateFileSystemDepth(int depth)
        {
            return new FileSystemDepth(
                depth);
        }
    }
}