using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Maple.Test.ViewModel.Navigation
{
    [TestClass]
    public class SceneTests
    {
        private MockRepository _mockRepository;

        private Mock<ILocalizationService> _mockTranslationService;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockTranslationService = _mockRepository.Create<ILocalizationService>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mockRepository.VerifyAll();
        }

        [TestMethod]
        public void SceneCtorTest()
        {
            var scene = CreateScene();
        }

        private Scene CreateScene()
        {
            return new Scene(
                _mockTranslationService.Object);
        }
    }
}