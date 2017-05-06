using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Maple.Test.Services
{
    [TestClass]
    public class SequenceServiceTests
    {
        private MockRepository _mockRepository;

        private Mock<IMapleLog> _mockMapleLog;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockMapleLog = _mockRepository.Create<IMapleLog>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mockRepository.VerifyAll();
        }

        [TestMethod]
        public void ServiceCtorTest()
        {
            var service = CreateService();
        }

        private SequenceService CreateService()
        {
            return new SequenceService(
                _mockMapleLog.Object);
        }
    }
}