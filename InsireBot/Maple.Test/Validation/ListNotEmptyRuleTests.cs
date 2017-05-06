using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Maple.Test.Validation
{
    [TestClass]
    public class ListNotEmptyRuleTests
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
        public void ListNotEmptyRuleCtorTest()
        {
            var listNotEmptyRule = CreateListNotEmptyRule(string.Empty);
        }

        private ListNotEmptyRule CreateListNotEmptyRule(string name)
        {
            return new ListNotEmptyRule(name);
        }
    }
}