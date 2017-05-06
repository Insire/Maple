using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Maple.Test.ViewModel
{
    [TestClass]
    public class PlaylistsTests
    {
        private MockRepository _mockRepository;

        private Mock<ITranslationService> _mockTranslationService;
        private Mock<IMapleLog> _mockMapleLog;
        private Mock<IPlaylistMapper> _mockPlaylistMapper;
        private Mock<Func<IMediaRepository>> _mockFunc;
        private Mock<ISequenceProvider> _mockSequenceProvider;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockTranslationService = _mockRepository.Create<ITranslationService>();
            _mockMapleLog = _mockRepository.Create<IMapleLog>();
            _mockPlaylistMapper = _mockRepository.Create<IPlaylistMapper>();
            _mockFunc = _mockRepository.Create<Func<IMediaRepository>>();
            _mockSequenceProvider = _mockRepository.Create<ISequenceProvider>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mockRepository.VerifyAll();
        }

        [TestMethod]
        public void PlaylistsCtorTest()
        {
            var playlists = CreatePlaylists();
        }

        private Playlists CreatePlaylists()
        {
            return new Playlists(
                _mockTranslationService.Object,
                _mockMapleLog.Object,
                _mockPlaylistMapper.Object,
                _mockFunc.Object,
                _mockSequenceProvider.Object);
        }
    }
}