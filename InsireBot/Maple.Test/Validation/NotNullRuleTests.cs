using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Maple.Test.Validation
{
    [TestClass]
    public class NotNullRuleTests
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
        public void NotNullRuleCtorTest()
        {
            var notNullRule = CreateNotNullRule(string.Empty);
        }

        private NotNullRule CreateNotNullRule(string name)
        {
            return new NotNullRule(name);
        }
    }
}