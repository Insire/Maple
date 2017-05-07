using Maple.Core;
using Maple.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Maple.Test.Utils.Mappers
{
    [TestClass]
    public class MediaItemMapperTests
    {
        private MockRepository _mockRepository;

        private Mock<IPlaylistContext> _mockPlaylistContext;
        private Mock<ITranslationService> _mockTranslationService;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockPlaylistContext = _mockRepository.Create<IPlaylistContext>();
            _mockTranslationService = _mockRepository.Create<ITranslationService>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mockRepository.VerifyAll();
        }

        [TestMethod]
        public void MediaItemMapperCtorTest()
        {
            var mediaItemMapper = CreateMediaItemMapper();
        }

        private MediaItemMapper CreateMediaItemMapper()
        {
            return new MediaItemMapper(
                _mockPlaylistContext.Object,
                _mockTranslationService.Object);
        }
    }
}