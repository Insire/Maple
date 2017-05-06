using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;

namespace Maple.Test.IO
{
    [TestClass]
    public class MapleDriveTests
    {
        private MockRepository _mockRepository;

        private Mock<DriveInfo> _mockDriveInfo;
        private Mock<IDepth> _mockDepth;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockDriveInfo = _mockRepository.Create<DriveInfo>();
            _mockDepth = _mockRepository.Create<IDepth>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mockRepository.VerifyAll();
        }

        [TestMethod]
        public void MapleDriveCtorTest()
        {
            var mapleDrive = CreateMapleDrive();
        }

        private MapleDrive CreateMapleDrive()
        {
            return new MapleDrive(
                _mockDriveInfo.Object,
                _mockDepth.Object);
        }
    }
}