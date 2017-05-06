using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Maple.Test.Observables
{
    [TestClass]
    public class BusyTokenTests
    {
        private MockRepository _mockRepository;

        private Mock<BusyStack> _mockBusyStack;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockBusyStack = _mockRepository.Create<BusyStack>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mockRepository.VerifyAll();
        }

        [TestMethod]
        public void BusyTokenCtorTest()
        {
            var busyToken = CreateBusyToken();
        }

        private BusyToken CreateBusyToken()
        {
            return new BusyToken(
                _mockBusyStack.Object);
        }
    }
}