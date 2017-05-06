using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Maple.Test.Validation
{
    [TestClass]
    public class NotFalseRuleTests
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
        public void NotFalseRuleCtorTest()
        {
            var notFalseRule = CreateNotFalseRule(string.Empty);
        }

        private NotFalseRule CreateNotFalseRule(string name)
        {
            return new NotFalseRule(name);
        }
    }
}