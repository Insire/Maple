using Maple.Youtube;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Maple.Test.ViewModel
{
    [TestClass]
    public class CreatePlaylistTests
    {
        private MockRepository _mockRepository;

        private Mock<IYoutubeUrlParseService> _mockYoutubeUrlParseService;
        private Mock<IPlaylistMapper> _mockPlaylistMapper;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockYoutubeUrlParseService = _mockRepository.Create<IYoutubeUrlParseService>();
            _mockPlaylistMapper = _mockRepository.Create<IPlaylistMapper>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mockRepository.VerifyAll();
        }

        [TestMethod]
        public void CreatePlaylistCtorTest()
        {
            var createPlaylist = CreateCreatePlaylist();
        }

        private CreatePlaylist CreateCreatePlaylist()
        {
            return new CreatePlaylist(
                _mockYoutubeUrlParseService.Object,
                _mockPlaylistMapper.Object);
        }
    }
}