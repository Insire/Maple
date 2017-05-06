using Maple.Core;
using Maple.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Maple.Test.ViewModel.MediaItems
{
    [TestClass]
    public class MediaItemTests
    {
        private MockRepository _mockRepository;
        private Mock<IPlaylistContext> _mockPlaylistContext;
        private Mock<ITranslationService> _mockITranslationService;
        private MediaItemMapper _mapper;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockPlaylistContext = _mockRepository.Create<IPlaylistContext>();
            _mockITranslationService = _mockRepository.Create<ITranslationService>();

            _mapper = new MediaItemMapper(_mockPlaylistContext.Object, _mockITranslationService.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mockRepository.VerifyAll();
        }

        [TestMethod]
        public void MediaItemCtorTest()
        {
            var mediaItem = CreateMediaItem();
        }

        private MediaItem CreateMediaItem()
        {
            return new MediaItem(
                _mapper.GetDataNewMediaItem(0),
                _mockPlaylistContext.Object);
        }
    }
}