using Maple;
using Maple.Youtube;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Maple.Test.ViewModel.MediaItems
{
    [TestClass]
    public class CreateMediaItemTests
    {
        private MockRepository _mockRepository;

        private Mock<IYoutubeUrlParseService> _mockYoutubeUrlParseService;
        private Mock<IMediaItemMapper> _mockMediaItemMapper;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockYoutubeUrlParseService = _mockRepository.Create<IYoutubeUrlParseService>();
            _mockMediaItemMapper = _mockRepository.Create<IMediaItemMapper>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mockRepository.VerifyAll();
        }

        [TestMethod]
        public void CreateMediaItemCtorTest()
        {
            var createMediaItem = CreateCreateMediaItem();
        }

        private CreateMediaItem CreateCreateMediaItem()
        {
            return new CreateMediaItem(
                _mockYoutubeUrlParseService.Object,
                _mockMediaItemMapper.Object);
        }
    }
}