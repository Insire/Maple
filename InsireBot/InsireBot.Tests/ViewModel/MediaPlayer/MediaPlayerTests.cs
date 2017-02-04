using Maple.Core;
using Maple.Data;
using Maple.Localization.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Maple.Tests
{
    [TestClass()]
    public class MediaPlayerTests
    {
        private static MediaPlayerRepository _mediaPlayerRepository;
        private static PlaylistsRepository _playlistsRepository;
        private static IBotLog _log;
        private static ITranslationManager _translationManager;
        private static IMediaItemRepository _mediaItemRepository;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            var connection = new DBConnection(context.DeploymentDirectory);
            _mediaPlayerRepository = new MediaPlayerRepository(connection);
            _playlistsRepository = new PlaylistsRepository(connection, _mediaItemRepository);
            _mediaItemRepository = new MediaItemRepository(connection);
            _log = new MockLog();
            _translationManager = new MockTranslationManager();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MainMediaPlayerCtorEmptyManagerTest()
        {
            try
            {
                var mediaplayer = new MainMediaPlayer(null, CreateMockMediaPlayer(), CreateOneDataMediaPlayer(), "");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("manager", ex.ParamName);
                throw;
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MainMediaPlayerCtorEmptyPlayerTest()
        {
            try
            {
                var mediaplayer = new MainMediaPlayer(_translationManager, null, CreateOneDataMediaPlayer(), "");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("player", ex.ParamName);
                throw;
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MainMediaPlayerCtorEmptyMediaPlayerTest()
        {
            try
            {
                var mediaplayer = new MainMediaPlayer(_translationManager, CreateMockMediaPlayer(), null, "");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("model", ex.ParamName);
                throw;
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MainMediaPlayerCtorEmptyNameKeyTest()
        {
            try
            {
                var mediaplayer = new MainMediaPlayer(_translationManager, CreateMockMediaPlayer(), CreateOneDataMediaPlayer(), "");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("nameKey", ex.ParamName);
                throw;
            }
        }

        [TestMethod()]
        public void MainMediaPlayerTest()
        {
            var mediaplayer = new MainMediaPlayer(_translationManager, CreateMockMediaPlayer(), CreateOneDataMediaPlayer(), nameof(Resources.MainMediaplayer));
            mediaplayer.Playlist = new Playlist(_log, _playlistsRepository, _mediaItemRepository, new DialogViewModel(new MockYoutubeParseService(), new MediaItemMapper(_log, _mediaItemRepository)), CreatePlaylist());

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
            var mediaplayer = new MainMediaPlayer(_translationManager, CreateMockMediaPlayer(), CreateOneNewDataMediaPlayer(), nameof(Resources.MainMediaplayer));
            mediaplayer.Playlist = new Playlist(_log, _playlistsRepository, _mediaItemRepository, new DialogViewModel(new MockYoutubeParseService(), new MediaItemMapper(_log, _mediaItemRepository)), CreatePlaylist());

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
            mediaplayer.Playlist = new Playlist(_log, _playlistsRepository, _mediaItemRepository, new DialogViewModel(new MockYoutubeParseService(), new MediaItemMapper(_log, _mediaItemRepository)), CreatePlaylist());

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
            mediaplayer.Playlist = new Playlist(_log, _playlistsRepository, _mediaItemRepository, new DialogViewModel(new MockYoutubeParseService(), new MediaItemMapper(_log, _mediaItemRepository)), CreatePlaylist());

            Assert.IsTrue(mediaplayer.IsValid);
            Assert.IsTrue(mediaplayer.IsChanged);

            Assert.IsFalse(mediaplayer.IsPrimary);
            Assert.IsFalse(mediaplayer.IsPlaying);
            Assert.IsFalse(mediaplayer.HasErrors);

            Assert.AreEqual("Test", mediaplayer.Name);

            Assert.IsNotNull(mediaplayer.AudioDevices);
        }

        [TestMethod()]
        public void MediaPlayerNameChangeTest()
        {
            var mediaplayer = new MediaPlayer(CreateMockMediaPlayer(), CreateOneNewDataMediaPlayer());
            mediaplayer.Playlist = new Playlist(_log, _playlistsRepository, _mediaItemRepository, new DialogViewModel(new MockYoutubeParseService(), new MediaItemMapper(_log, _mediaItemRepository)), CreatePlaylist());

            Assert.IsTrue(mediaplayer.IsValid);
            Assert.IsTrue(mediaplayer.IsChanged);

            Assert.IsFalse(mediaplayer.IsPrimary);
            Assert.IsFalse(mediaplayer.IsPlaying);
            Assert.IsFalse(mediaplayer.HasErrors);

            Assert.AreEqual("Test", mediaplayer.Name);

            Assert.IsNotNull(mediaplayer.AudioDevices);

            mediaplayer.AcceptChanges();

            mediaplayer.Name = "Changed";
            Assert.AreEqual("Changed", mediaplayer.Model.Name);
            Assert.IsTrue(mediaplayer.IsChanged);
            Assert.IsTrue(mediaplayer.IsValid);
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
                PrivacyStatus = 0,
                IsShuffeling = false,
                RepeatMode = 0,
                Sequence = 0,
                MediaItems = new List<Data.MediaItem>(),
                IsDeleted = false,
            };
        }
    }
}