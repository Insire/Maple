using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Maple.Test.Observables
{
    [TestClass]
    public class BusyStackTests
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
        public void BusyStackCtorTest()
        {


            var busyStack = CreateBusyStack();


        }

        private BusyStack CreateBusyStack()
        {
            return new BusyStack();
        }
    }
}