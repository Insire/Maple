using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Maple.Test.Utils.Mappers
{
    [TestClass]
    public class MediaPlayerMapperTests
    {
        private MockRepository _mockRepository;

        private Mock<ITranslationService> _mockTranslationService;
        private Mock<IMediaPlayer> _mockMediaPlayer;
        private Mock<AudioDevices> _mockAudioDevices;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockTranslationService = _mockRepository.Create<ITranslationService>();
            _mockMediaPlayer = _mockRepository.Create<IMediaPlayer>();
            _mockAudioDevices = _mockRepository.Create<AudioDevices>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mockRepository.VerifyAll();
        }

        [TestMethod]
        public void MediaPlayerMapperCtorTest()
        {
            var mediaPlayerMapper = CreateMediaPlayerMapper();
        }

        private MediaPlayerMapper CreateMediaPlayerMapper()
        {
            return new MediaPlayerMapper(
                _mockTranslationService.Object,
                _mockMediaPlayer.Object,
                _mockAudioDevices.Object);
        }
    }
}