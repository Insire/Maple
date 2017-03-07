using DryIoc;
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
        private static IContainer _container;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            var connection = new DBConnection(context.DeploymentDirectory);
            _container = new Container(rules => rules.WithoutThrowOnRegisteringDisposableTransient());

            _container.RegisterInstance(connection);

            _container.Register<IBotLog, MockLog>(reuse: Reuse.Singleton);

            _container.Register<Scenes>(reuse: Reuse.Singleton);
            _container.Register<UIColorsViewModel>(reuse: Reuse.Singleton);

            _container.Register<Playlists>(reuse: Reuse.Singleton);
            _container.Register<MediaPlayers>(reuse: Reuse.Singleton);
            _container.Register<ShellViewModel>(reuse: Reuse.Singleton);
            _container.Register<DialogViewModel>(reuse: Reuse.Singleton);
            _container.Register<OptionsViewModel>(reuse: Reuse.Singleton);
            _container.Register<DirectorViewModel>(reuse: Reuse.Singleton);
            _container.Register<StatusbarViewModel>(reuse: Reuse.Singleton);

            _container.Register<ITranslationProvider, ResxTranslationProvider>(reuse: Reuse.Singleton);
            _container.Register<ITranslationManager, MockTranslationManager>(reuse: Reuse.Singleton);
            _container.Register<IMediaPlayer, NAudioMediaPlayer>(reuse: Reuse.Transient);

            _container.Register<IMediaItemMapper, MediaItemMapper>();
            _container.Register<IPlaylistMapper, PlaylistMapper>();

            _container.Register<IPlaylistsRepository, PlaylistsRepository>(reuse: Reuse.Singleton);
            _container.Register<IMediaItemRepository, MediaItemRepository>(reuse: Reuse.Singleton);
            _container.Register<IMediaPlayerRepository, MediaPlayerRepository>(reuse: Reuse.Singleton);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MainMediaPlayerCtorEmptyManagerTest()
        {
            try
            {
                var playlist = new Playlist(_container.Resolve<IPlaylistsRepository>(),
_container.Resolve<IMediaItemRepository>(),
new DialogViewModel(new MockYoutubeParseService(), _container.Resolve<IMediaItemMapper>()), CreatePlaylist());

                var mediaplayer = new MainMediaPlayer(null, _container.Resolve<IMediaPlayerRepository>(), CreateMockMediaPlayer(), CreateOneDataMediaPlayer(), playlist, "");
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
                var playlist = new Playlist(_container.Resolve<IPlaylistsRepository>(),
_container.Resolve<IMediaItemRepository>(),
new DialogViewModel(new MockYoutubeParseService(), _container.Resolve<IMediaItemMapper>()), CreatePlaylist());

                var mediaplayer = new MainMediaPlayer(_container.Resolve<ITranslationManager>(), _container.Resolve<IMediaPlayerRepository>(), null, CreateOneDataMediaPlayer(), playlist, "test");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("player", ex.ParamName);
                throw;
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MainMediaPlayerCtorEmptyModelTest()
        {
            try
            {
                var playlist = new Playlist(_container.Resolve<IPlaylistsRepository>(),
_container.Resolve<IMediaItemRepository>(),
new DialogViewModel(new MockYoutubeParseService(), _container.Resolve<IMediaItemMapper>()), CreatePlaylist());

                var mediaplayer = new MainMediaPlayer(_container.Resolve<ITranslationManager>(), _container.Resolve<IMediaPlayerRepository>(), CreateMockMediaPlayer(), null, playlist, "test");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("model", ex.ParamName);
                throw;
            }
        }


        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MainMediaPlayerCtorEmptyRepositoryTest()
        {
            try
            {
                var playlist = new Playlist(_container.Resolve<IPlaylistsRepository>(),
        _container.Resolve<IMediaItemRepository>(),
        new DialogViewModel(new MockYoutubeParseService(), _container.Resolve<IMediaItemMapper>()),
        CreatePlaylist());

                var mediaplayer = new MainMediaPlayer(_container.Resolve<ITranslationManager>(), null, CreateMockMediaPlayer(), CreateOneDataMediaPlayer(), playlist, "test");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("repository", ex.ParamName);
                throw;
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MainMediaPlayerCtorEmptyNameKeyTest()
        {
            try
            {
                var playlist = new Playlist(_container.Resolve<IPlaylistsRepository>(),
                        _container.Resolve<IMediaItemRepository>(),
                        new DialogViewModel(new MockYoutubeParseService(), _container.Resolve<IMediaItemMapper>()),
                        CreatePlaylist());

                var mediaplayer = new MainMediaPlayer(_container.Resolve<ITranslationManager>(),
                                                        _container.Resolve<IMediaPlayerRepository>(),
                                                        CreateMockMediaPlayer(),
                                                        CreateOneDataMediaPlayer(),
                                                        playlist,
                                                        "");
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
            var playlist = new Playlist(_container.Resolve<IPlaylistsRepository>(),
                                    _container.Resolve<IMediaItemRepository>(),
                                    new DialogViewModel(new MockYoutubeParseService(), _container.Resolve<IMediaItemMapper>()),
                                    CreatePlaylist());
            var mediaplayer = new MainMediaPlayer(_container.Resolve<ITranslationManager>(),
                                                    _container.Resolve<IMediaPlayerRepository>(),
                                                    CreateMockMediaPlayer(),
                                                    CreateOneDataMediaPlayer(),
                                                    playlist,
                                                    nameof(Resources.MainMediaplayer));

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
            var playlist = new Playlist(_container.Resolve<IPlaylistsRepository>(),
                                                _container.Resolve<IMediaItemRepository>(),
                                                new DialogViewModel(new MockYoutubeParseService(), _container.Resolve<IMediaItemMapper>()),
                                                CreatePlaylist());

            var mediaplayer = new MainMediaPlayer(_container.Resolve<ITranslationManager>(),
                                                    _container.Resolve<IMediaPlayerRepository>(),
                                                    CreateMockMediaPlayer(),
                                                    CreateOneDataMediaPlayer(),
                                                    playlist,
                                                    nameof(Resources.MainMediaplayer));

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
            var mediaplayer = new MediaPlayer(_container.Resolve<ITranslationManager>(),
                                                    _container.Resolve<IMediaPlayerRepository>(),
                                                    CreateMockMediaPlayer(),
                                                    CreateOneDataMediaPlayer())
            {
                Playlist = new Playlist(_container.Resolve<IPlaylistsRepository>(),
                                                _container.Resolve<IMediaItemRepository>(),
                                                new DialogViewModel(new MockYoutubeParseService(), _container.Resolve<IMediaItemMapper>()),
                                                CreatePlaylist())
            };
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
            var mediaplayer = new MediaPlayer(_container.Resolve<ITranslationManager>(),
                                                    _container.Resolve<IMediaPlayerRepository>(),
                                                    CreateMockMediaPlayer(),
                                                    CreateOneDataMediaPlayer())
            {
                Playlist = new Playlist(_container.Resolve<IPlaylistsRepository>(),
                                                _container.Resolve<IMediaItemRepository>(),
                                                new DialogViewModel(new MockYoutubeParseService(), _container.Resolve<IMediaItemMapper>()),
                                                CreatePlaylist())
            };
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
            var mediaplayer = new MediaPlayer(_container.Resolve<ITranslationManager>(),
                                                    _container.Resolve<IMediaPlayerRepository>(),
                                                    CreateMockMediaPlayer(),
                                                    CreateOneDataMediaPlayer())
            {
                Playlist = new Playlist(_container.Resolve<IPlaylistsRepository>(),
                                                _container.Resolve<IMediaItemRepository>(),
                                                new DialogViewModel(new MockYoutubeParseService(), _container.Resolve<IMediaItemMapper>()),
                                                CreatePlaylist())
            };
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
                Volume = 50,
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