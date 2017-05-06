using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Maple.Test.Validation
{
    [TestClass]
    public class NotNullOrEmptyRuleTests
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
        public void NotNullOrEmptyRuleCtorTest()
        {
            var notNullOrEmptyRule = CreateNotNullOrEmptyRule(string.Empty);
        }

        private NotNullOrEmptyRule CreateNotNullOrEmptyRule(string name)
        {
            return new NotNullOrEmptyRule(name);
        }
    }
}