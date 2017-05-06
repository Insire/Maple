using Maple;
using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Maple.Test.Utils.Mappers
{
    [TestClass]
    public class PlaylistMapperTests
    {
        private MockRepository _mockRepository;

        private Mock<IMapleLog> _mockMapleLog;
        private Mock<ITranslationService> _mockTranslationService;
        private Mock<IMediaItemMapper> _mockMediaItemMapper;
        private Mock<DialogViewModel> _mockDialogViewModel;
        private Mock<ISequenceProvider> _mockSequenceProvider;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockMapleLog = _mockRepository.Create<IMapleLog>();
            _mockTranslationService = _mockRepository.Create<ITranslationService>();
            _mockMediaItemMapper = _mockRepository.Create<IMediaItemMapper>();
            _mockDialogViewModel = _mockRepository.Create<DialogViewModel>();
            _mockSequenceProvider = _mockRepository.Create<ISequenceProvider>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mockRepository.VerifyAll();
        }

        [TestMethod]
        public void PlaylistMapperCtorTest()
        {
            var playlistMapper = CreatePlaylistMapper();
        }

        private PlaylistMapper CreatePlaylistMapper()
        {
            return new PlaylistMapper(
                _mockMapleLog.Object,
                _mockTranslationService.Object,
                _mockMediaItemMapper.Object,
                _mockDialogViewModel.Object,
                _mockSequenceProvider.Object);
        }
    }
}