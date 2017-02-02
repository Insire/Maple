using Maple.Core;
using Maple.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Maple.Tests
{
    [TestClass()]
    public class MediaPlayerTests
    {
        private static MediaPlayerRepository _repository;
        private static IBotLog _log;
        private static ITranslationManager _translationManager;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _repository = new MediaPlayerRepository(new DBConnection(context.DeploymentDirectory));
            _log = new MockLog();
            _translationManager = new MockTranslationManager();
        }

        [TestMethod()]
        public void MainMediaPlayerTest()
        {
            var mediaplayer = new MainMediaPlayer(_translationManager, CreateMockMediaPlayer(), CreateOneDataMediaPlayer(), "");
            mediaplayer.Playlist = new Playlist(_log, CreatePlaylist());

            Assert.IsTrue(mediaplayer.IsValid);
            Assert.IsTrue(mediaplayer.IsPrimary);
            Assert.IsTrue(mediaplayer.IsChanged);

            Assert.IsFalse(mediaplayer.IsPlaying);
            Assert.IsFalse(mediaplayer.HasErrors);

            Assert.AreEqual("MockTranslation", mediaplayer.Name);

            Assert.IsNotNull(mediaplayer.AudioDevices);
        }

        [TestMethod()]
        public void NewMainMediaPlayerTest()
        {
            var mediaplayer = new MainMediaPlayer(_translationManager, CreateMockMediaPlayer(), CreateOneNewDataMediaPlayer(), "");
            mediaplayer.Playlist = new Playlist(_log, CreatePlaylist());

            Assert.IsTrue(mediaplayer.IsValid);
            Assert.IsTrue(mediaplayer.IsChanged);
            Assert.IsTrue(mediaplayer.IsPrimary);

            Assert.IsFalse(mediaplayer.IsPlaying);
            Assert.IsFalse(mediaplayer.HasErrors);

            Assert.AreEqual("MockTranslation", mediaplayer.Name);

            Assert.IsNotNull(mediaplayer.AudioDevices);
        }

        [TestMethod()]
        public void MediaPlayerTest()
        {
            var mediaplayer = new MediaPlayer(CreateMockMediaPlayer(), CreateOneDataMediaPlayer());
            mediaplayer.Playlist = new Playlist(_log, CreatePlaylist());

            Assert.IsTrue(mediaplayer.IsValid);
            Assert.IsTrue(mediaplayer.IsChanged);

            Assert.IsFalse(mediaplayer.IsPrimary);
            Assert.IsFalse(mediaplayer.IsPlaying);
            Assert.IsFalse(mediaplayer.HasErrors);

            Assert.AreEqual("Test", mediaplayer.Name);

            Assert.IsNotNull(mediaplayer.AudioDevices);
        }

        [TestMethod()]
        public void NewMediaPlayerTest()
        {
            var mediaplayer = new MediaPlayer(CreateMockMediaPlayer(), CreateOneNewDataMediaPlayer());
            mediaplayer.Playlist = new Playlist(_log, CreatePlaylist());

            Assert.IsTrue(mediaplayer.IsValid);
            Assert.IsTrue(mediaplayer.IsChanged);

            Assert.IsFalse(mediaplayer.IsPrimary);
            Assert.IsFalse(mediaplayer.IsPlaying);
            Assert.IsFalse(mediaplayer.HasErrors);

            Assert.AreEqual("Test", mediaplayer.Name);

            Assert.IsNotNull(mediaplayer.AudioDevices);
        }

        [TestMethod()]
        public void AddRangeTest()
        {
            // TODO
            Assert.Fail();
        }

        [TestMethod()]
        public void AddTest()
        {
            // TODO
            Assert.Fail();
        }

        [TestMethod()]
        public void PauseTest()
        {
            // TODO
            Assert.Fail();
        }

        [TestMethod()]
        public void StopTest()
        {
            // TODO
            Assert.Fail();
        }

        [TestMethod()]
        public void PreviousTest()
        {
            // TODO
            Assert.Fail();
        }

        [TestMethod()]
        public void NextTest()
        {
            // TODO
            Assert.Fail();
        }

        [TestMethod()]
        public void CanNextTest()
        {
            // TODO
            Assert.Fail();
        }

        [TestMethod()]
        public void DisposeTest()
        {
            // TODO
            Assert.Fail();
        }

        [TestMethod()]
        public void DisposeTest1()
        {
            // TODO
            Assert.Fail();
        }

        private Data.MediaPlayer CreateOneDataMediaPlayer()
        {
            return new Data.MediaPlayer
            {
                DeviceName = "TestMediaPlayer",
                IsDeleted = false,
                IsPrimary = true,
                Name = "Test",
                PlaylistId = 0,
                Sequence = 0,
                Id = 1,
            };
        }

        private Data.MediaPlayer CreateOneNewDataMediaPlayer()
        {
            return new Data.MediaPlayer
            {
                DeviceName = "TestMediaPlayer",
                IsDeleted = false,
                IsPrimary = true,
                Name = "Test",
                PlaylistId = 0,
                Sequence = 0,
            };
        }

        private MockMediaPlayer CreateMockMediaPlayer()
        {
            return new MockMediaPlayer
            {
                AudioDevice = new AudioDevice("Test", 2),
                IsPlaying = false,
                Volume = 50,
                VolumeMax = 100,
                VolumeMin = 0,
            };
        }

        private Data.Playlist CreatePlaylist()
        {
            return new Data.Playlist
            {
                Title = "TestPlaylist",
                Description = "Description",
                IsRestricted = false,
                IsShuffeling = false,
                RepeatMode = 0,
                Sequence = 0,
                MediaItems = new List<Data.MediaItem>(),
                IsDeleted = false,
            };
        }
    }
}