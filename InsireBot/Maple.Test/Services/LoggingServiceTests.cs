using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Maple.Test.Services
{
    [TestClass]
    public class LoggingServiceTests
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
        public void ServiceCtorTest()
        {
            var service = CreateService();
        }

        private LoggingService CreateService()
        {
            return new LoggingService();
        }
    }
}