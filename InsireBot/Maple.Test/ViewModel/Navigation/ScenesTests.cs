using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Maple.Test.ViewModel.Navigation
{
    [TestClass]
    public class ScenesTests
    {
        private MockRepository _mockRepository;

        private Mock<ILocalizationService> _mockTranslationService;
        private Mock<IMapleLog> _mockMapleLog;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockTranslationService = _mockRepository.Create<ILocalizationService>();
            _mockMapleLog = _mockRepository.Create<IMapleLog>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mockRepository.VerifyAll();
        }

        [TestMethod]
        public void ScenesCtorTest()
        {
            var scenes = CreateScenes();
        }

        private Scenes CreateScenes()
        {
            return new Scenes(
                _mockTranslationService.Object,
                _mockMapleLog.Object);
        }
    }
}