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

        private Mock<IMediaItemMapper> _mockMediaItemMapper;
        private Mock<DialogViewModel> _mockDialogViewModel;
        private Mock<ITranslationService> _mockTranslationService;
        private Mock<ISequenceProvider> _mockSequenceProvider;
        private Mock<IMapleLog> _mockMapleLog;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockMediaItemMapper = _mockRepository.Create<IMediaItemMapper>();
            _mockDialogViewModel = _mockRepository.Create<DialogViewModel>();
            _mockTranslationService = _mockRepository.Create<ITranslationService>();
            _mockSequenceProvider = _mockRepository.Create<ISequenceProvider>();
            _mockMapleLog = _mockRepository.Create<IMapleLog>();
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
                _mockMediaItemMapper.Object,
                _mockDialogViewModel.Object,
                _mockTranslationService.Object,
                _mockSequenceProvider.Object,
                _mockMapleLog.Object);
        }
    }
}