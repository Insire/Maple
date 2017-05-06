using Maple;
using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Maple.Test.ViewModel
{
    [TestClass]
    public class ShellViewModelTests
    {
        private MockRepository _mockRepository;

        private Mock<ITranslationService> _mockTranslationService;
        private Mock<Scenes> _mockScenes;
        private Mock<StatusbarViewModel> _mockStatusbarViewModel;
        private Mock<DialogViewModel> _mockDialogViewModel;
        private Mock<IPlaylistsViewModel> _mockPlaylistsViewModel;
        private Mock<IMediaPlayersViewModel> _mockMediaPlayersViewModel;
        private Mock<OptionsViewModel> _mockOptionsViewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockTranslationService = _mockRepository.Create<ITranslationService>();
            _mockScenes = _mockRepository.Create<Scenes>();
            _mockStatusbarViewModel = _mockRepository.Create<StatusbarViewModel>();
            _mockDialogViewModel = _mockRepository.Create<DialogViewModel>();
            _mockPlaylistsViewModel = _mockRepository.Create<IPlaylistsViewModel>();
            _mockMediaPlayersViewModel = _mockRepository.Create<IMediaPlayersViewModel>();
            _mockOptionsViewModel = _mockRepository.Create<OptionsViewModel>();
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

        private ShellViewModel CreateViewModel()
        {
            return new ShellViewModel(
                _mockTranslationService.Object,
                _mockScenes.Object,
                _mockStatusbarViewModel.Object,
                _mockDialogViewModel.Object,
                _mockPlaylistsViewModel.Object,
                _mockMediaPlayersViewModel.Object,
                _mockOptionsViewModel.Object);
        }
    }
}