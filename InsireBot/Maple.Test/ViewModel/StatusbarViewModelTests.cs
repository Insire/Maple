using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Maple.Test.ViewModel
{
    [TestClass]
    public class StatusbarViewModelTests
    {
        private MockRepository _mockRepository;

        private Mock<ILocalizationService> _mockTranslationService;
        private Mock<IVersionService> _mockVersionService;
        private Mock<IMediaPlayersViewModel> _mockMediaPlayersViewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockTranslationService = _mockRepository.Create<ILocalizationService>();
            _mockVersionService = _mockRepository.Create<IVersionService>();
            _mockMediaPlayersViewModel = _mockRepository.Create<IMediaPlayersViewModel>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mockRepository.VerifyAll();
        }

        [TestMethod]
        public void ViewModelCtorTest()
        {
            var viewModel = CreateViewModel();
        }

        private StatusbarViewModel CreateViewModel()
        {
            return new StatusbarViewModel(
                _mockTranslationService.Object,
                _mockVersionService.Object,
                _mockMediaPlayersViewModel.Object);
        }
    }
}