using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DryIoc;
using FluentValidation;
using FluentValidation.Results;
using Maple.Core;
using Maple.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Maple.Test.ViewModels.Playlists
{
    [TestClass, TestCategory("PlaylistTests")]
    public class PlaylistTests
    {
        private static IContainer _container;
        private static TestContext _context;
        private static ValidationResult _defaultValidationResult;

        [ClassInitialize]
        public static async Task ClassInitialize(TestContext context)
        {
            _context = context;

            _defaultValidationResult = new ValidationResult(Enumerable.Empty<ValidationFailure>());

            _container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            _container.UseInstance(CreateViewModelServiceContainer());
            _container.UseInstance(CreateLoggingService());
            _container.UseInstance(CreateILoggingNotifcationService());
            _container.UseInstance(CreateILocalizationService());
            _container.UseInstance(CreateIMessenger());
            _container.UseInstance(CreateISequenceService());

            _container.UseInstance(CreateDialogViewModel());
        }

        [TestMethod]
        public void Playlist_ShouldRunConstructorWithoutErrors()
        {
            var model = CreateModelPlaylist();
            var playlist = CreatePlaylist(model);

            Assert.AreEqual(_context.FullyQualifiedTestClassName, playlist.CreatedBy);
            Assert.AreEqual(_context.FullyQualifiedTestClassName, playlist.UpdatedBy);
            Assert.AreEqual(4, playlist.Count);
            Assert.AreEqual($"Description for {_context.FullyQualifiedTestClassName} Playlist", playlist.Description);
            Assert.AreEqual(false, playlist.HasErrors);
            Assert.AreEqual(1, playlist.Id);
            Assert.AreEqual(false, playlist.IsBusy);
            Assert.AreEqual(false, playlist.IsChanged);
            Assert.AreEqual(false, playlist.IsDeleted);
            Assert.AreEqual(false, playlist.IsNew);
            Assert.AreEqual(false, playlist.IsSelected);
            Assert.AreEqual(false, playlist.IsShuffeling);
            Assert.AreEqual(4, playlist.Items.Count);
            Assert.AreEqual(model, playlist.Model);
            Assert.AreEqual(PrivacyStatus.None, playlist.PrivacyStatus);
            Assert.AreEqual(RepeatMode.None, playlist.RepeatMode);
            Assert.AreEqual(playlist[0], playlist.SelectedItem);
            Assert.AreEqual(1, playlist.Sequence);
            Assert.AreEqual($"Title for {_context.FullyQualifiedTestClassName} Playlist", playlist.Title);

            Assert.IsNotNull(playlist.View);
            Assert.IsNotNull(playlist.ClearCommand);
            Assert.IsNotNull(playlist.LoadFromFileCommand);
            Assert.IsNotNull(playlist.LoadFromFolderCommand);
            Assert.IsNotNull(playlist.LoadFromUrlCommand);
            Assert.IsNotNull(playlist.RemoveCommand);
            Assert.IsNotNull(playlist.RemoveRangeCommand);
        }

        [TestMethod]
        public void Playlist_ShouldThrowForEmptyModel()
        {
            Assert.ThrowsException<ArgumentNullException>(() => CreatePlaylist(default(PlaylistModel)));
        }

        [TestMethod]
        public void Playlist_ShouldThrowForEmptyContainer()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new Playlist(null, _container.Resolve<IValidator<Playlist>>(), _container.Resolve<IDialogViewModel>(), _container.Resolve<IMediaItemMapper>(), CreateModelPlaylist()));
        }

        [TestMethod]
        public void Playlist_ShouldThrowForEmptyValidator()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new Playlist(_container.Resolve<ViewModelServiceContainer>(), null, _container.Resolve<IDialogViewModel>(), _container.Resolve<IMediaItemMapper>(), CreateModelPlaylist()));
        }

        [TestMethod]
        public void Playlist_ShouldThrowForEmptyViewModel()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new Playlist(_container.Resolve<ViewModelServiceContainer>(), _container.Resolve<IValidator<Playlist>>(), null, _container.Resolve<IMediaItemMapper>(), CreateModelPlaylist()));
        }

        [TestMethod]
        public void Playlist_ShouldThrowForEmptyMediaItemMapper()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new Playlist(_container.Resolve<ViewModelServiceContainer>(), _container.Resolve<IValidator<Playlist>>(), _container.Resolve<IDialogViewModel>(), null, CreateModelPlaylist()));
        }

        [TestMethod]
        public void Playlist_ShouldRunClear()
        {
            var playlist = CreatePlaylist(CreateModelPlaylist());

            Assert.AreEqual(4, playlist.Count);

            playlist.Clear();

            Assert.AreEqual(0, playlist.Count);
        }

        [TestMethod]
        public void Playlist_ShouldAdd()
        {
            var mediaItem = CreateMediaItem(CreateModelMediaItem());
            var playlist = CreatePlaylist(CreateModelPlaylist());

            Assert.AreEqual(4, playlist.Count);

            playlist.Add(mediaItem);

            Assert.AreEqual(5, playlist.Count);
        }

        [TestMethod]
        public void Playlist_ShouldThrowAddForNull()
        {
            var playlist = CreatePlaylist(CreateModelPlaylist());

            Assert.ThrowsException<ArgumentNullException>(() => playlist.Add(null));
        }

        [TestMethod]
        public void Playlist_ShouldAddRange()
        {
            var mediaItems = new List<MediaItem>()
            {
                CreateMediaItem(CreateModelMediaItem()),
                CreateMediaItem(CreateModelMediaItem()),
                CreateMediaItem(CreateModelMediaItem()),
            };
            var playlist = CreatePlaylist(CreateModelPlaylist());

            Assert.AreEqual(4, playlist.Count);

            playlist.AddRange(mediaItems);

            Assert.AreEqual(7, playlist.Count);
        }

        [TestMethod]
        public void Playlist_ShouldHandleAddRangeForEmptyCollection()
        {
            var mediaItems = new List<MediaItem>();
            var playlist = CreatePlaylist(CreateModelPlaylist());

            Assert.AreEqual(4, playlist.Count);

            playlist.AddRange(mediaItems);

            Assert.AreEqual(4, playlist.Count);
        }

        [TestMethod]
        public void Playlist_ShouldThrowAddRangeForNull()
        {
            var playlist = CreatePlaylist(CreateModelPlaylist());

            Assert.AreEqual(4, playlist.Count);

            Assert.ThrowsException<ArgumentNullException>(() => playlist.AddRange(null));
        }

        [TestMethod]
        public void Playlist_ShouldHandleAddRangeForDuplicateEntries()
        {
            var mediaItem = CreateMediaItem(CreateModelMediaItem());
            var mediaItems = new List<MediaItem>()
            {
                mediaItem,
                mediaItem,
                mediaItem,
            };
            var playlist = CreatePlaylist(CreateModelPlaylist());

            Assert.AreEqual(4, playlist.Count);

            playlist.AddRange(mediaItems);

            Assert.AreEqual(7, playlist.Count);
        }

        [TestMethod]
        public void Playlist_ShouldRemove()
        {
            var playlist = CreatePlaylist(CreateModelPlaylist());

            Assert.AreEqual(4, playlist.Count);

            playlist.Remove(playlist[0]);

            Assert.AreEqual(3, playlist.Count);
        }

        [TestMethod]
        public void Playlist_ShouldThrowRemoveForNull()
        {
            var playlist = CreatePlaylist(CreateModelPlaylist());

            Assert.ThrowsException<ArgumentNullException>(() => playlist.Remove(null));
        }

        [TestMethod]
        public void Playlist_ShouldRemoveRange()
        {
            var playlist = CreatePlaylist(CreateModelPlaylist());
            var mediaItems = new List<MediaItem>()
            {
                playlist[0],
                playlist[1],
                playlist[2],
            };

            Assert.AreEqual(4, playlist.Count);

            playlist.RemoveRange(mediaItems);

            Assert.AreEqual(1, playlist.Count);
        }

        [TestMethod]
        public void Playlist_ShouldThrowRemoveRangeForNull()
        {
            var playlist = CreatePlaylist(CreateModelPlaylist());

            Assert.ThrowsException<ArgumentNullException>(() => playlist.RemoveRange(null));
        }

        [TestMethod]
        public void Playlist_ShouldHandleRemoveRangeForSameItem()
        {
            var playlist = CreatePlaylist(CreateModelPlaylist());
            var mediaItems = new List<MediaItem>()
            {
                playlist[0],
                playlist[0],
                playlist[0],
            };

            Assert.AreEqual(4, playlist.Count);

            playlist.RemoveRange(mediaItems);

            Assert.AreEqual(3, playlist.Count);
        }

        [TestMethod]
        public void Playlist_ShouldHandleRemoveRangeForUnknownItem()
        {
            var playlist = CreatePlaylist(CreateModelPlaylist());
            var mediaItems = new List<MediaItem>()
            {
                CreateMediaItem(new MediaItemModel()),
            };

            Assert.AreEqual(4, playlist.Count);

            playlist.RemoveRange(mediaItems);

            Assert.AreEqual(4, playlist.Count);
        }

        [TestMethod]
        public void Playlist_ShouldHandleRemoveRangeForUnknownItems()
        {
            var playlist = CreatePlaylist(CreateModelPlaylist());
            var mediaItems = new List<MediaItem>()
            {
                CreateMediaItem(new MediaItemModel()),
                CreateMediaItem(new MediaItemModel()),
                CreateMediaItem(new MediaItemModel()),
            };

            Assert.AreEqual(4, playlist.Count);

            playlist.RemoveRange(mediaItems);

            Assert.AreEqual(4, playlist.Count);
        }

        [TestMethod]
        public void Playlist_ShouldHandleRemoveRangeForEmptyCollection()
        {
            var playlist = CreatePlaylist(CreateModelPlaylist());
            var mediaItems = new List<MediaItem>();

            Assert.AreEqual(4, playlist.Count);

            playlist.RemoveRange(mediaItems);

            Assert.AreEqual(4, playlist.Count);
        }

        [TestMethod]
        public void Playlist_ShouldRunNext()
        {
            var playlist = CreatePlaylist(CreateModelPlaylist());
            playlist.RepeatMode = RepeatMode.None;
            var mediaItem = playlist.Next();

            Assert.IsNotNull(mediaItem);
            Assert.AreEqual(playlist[1], mediaItem);
            Assert.AreNotEqual(playlist.SelectedItem, mediaItem);
        }

        [TestMethod]
        public void Playlist_ShouldRunNextWithRepeatModeNone()
        {
            var playlist = CreatePlaylist(CreateModelPlaylist());
            playlist.RepeatMode = RepeatMode.None;
            playlist.SelectedItem = playlist[3];

            var mediaItem = playlist.Next();

            Assert.IsNull(mediaItem);
        }

        [TestMethod]
        public void Playlist_ShouldRunNextWithRepeatModeAll()
        {
            var playlist = CreatePlaylist(CreateModelPlaylist());
            playlist.RepeatMode = RepeatMode.All;
            playlist.SelectedItem = playlist[3];

            var mediaItem = playlist.Next();

            Assert.IsNotNull(mediaItem);
            Assert.AreEqual(playlist[0], mediaItem);
        }

        [TestMethod]
        public void Playlist_ShouldRunNextWithRepeatModeAllWhileShuffeling()
        {
            var playlist = CreatePlaylist(CreateModelPlaylist());
            playlist.RepeatMode = RepeatMode.All;
            playlist.IsShuffeling = true;
            playlist.SelectedItem = playlist[3];

            var mediaItem = playlist.Next();

            Assert.IsNotNull(mediaItem);
            Assert.AreNotEqual(playlist.SelectedItem, mediaItem);
        }

        [TestMethod]
        public void Playlist_ShouldRunNextWithRepeatModeNoneWhileShuffeling()
        {
            var playlist = CreatePlaylist(CreateModelPlaylist());
            playlist.RepeatMode = RepeatMode.None;
            playlist.IsShuffeling = true;
            playlist.SelectedItem = playlist[3];

            var mediaItem = playlist.Next();

            Assert.IsNotNull(mediaItem);
            Assert.AreNotEqual(playlist.SelectedItem, mediaItem);
        }

        [TestMethod]
        public void Playlist_ShouldRunNextWithRepeatModeSingleWhileShuffeling()
        {
            var playlist = CreatePlaylist(CreateModelPlaylist());
            playlist.RepeatMode = RepeatMode.Single;
            playlist.IsShuffeling = true;

            var mediaItem = playlist.Next();

            Assert.IsNotNull(mediaItem);
            Assert.AreEqual(playlist[0], mediaItem);
            Assert.AreEqual(playlist.SelectedItem, mediaItem);
        }

        [TestMethod]
        public void Playlist_ShouldRunNextWithRepeatModeSingle()
        {
            var playlist = CreatePlaylist(CreateModelPlaylist());
            playlist.RepeatMode = RepeatMode.Single;

            var mediaItem = playlist.Next();

            Assert.IsNotNull(mediaItem);
            Assert.AreEqual(playlist[0], mediaItem);
            Assert.AreEqual(playlist.SelectedItem, mediaItem);
        }

        [TestMethod]
        public void Playlist_ShouldRunPrevious()
        {
            throw new NotImplementedException();
            CreatePlaylist(CreateModelPlaylist());
        }

        [TestMethod]
        public void Playlist_ShouldRaiseSelectionChanging()
        {
            throw new NotImplementedException();
            CreatePlaylist(CreateModelPlaylist());
        }

        [TestMethod]
        public void Playlist_ShouldRaiseSelectionChanged()
        {
            throw new NotImplementedException();
            CreatePlaylist(CreateModelPlaylist());
        }

        [TestMethod]
        public void Playlist_ShouldSynchronizeItemsWithModel()
        {
            throw new NotImplementedException();
            CreatePlaylist(CreateModelPlaylist());
        }

        [TestMethod]
        public void Playlist_ShouldAddItemsFromFileDialog()
        {
            throw new NotImplementedException();
            CreatePlaylist(CreateModelPlaylist());
        }

        [TestMethod]
        public void Playlist_ShouldAddItemsFromFolderDialog()
        {
            throw new NotImplementedException();
            CreatePlaylist(CreateModelPlaylist());
        }

        [TestMethod]
        public void Playlist_ShouldAddItemsFromUrlDialog()
        {
            throw new NotImplementedException();
            CreatePlaylist(CreateModelPlaylist());
        }

        private MediaItemModel CreateModelMediaItem()
        {
            return new MediaItemModel()
            {
                CreatedBy = _context.FullyQualifiedTestClassName,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                UpdatedBy = _context.FullyQualifiedTestClassName,
                Description = $"Description for {_context.FullyQualifiedTestClassName} single MediaItem",
                Duration = 0,
                Id = 1,
                IsDeleted = false,
                Location = "Memory",
                PrivacyStatus = (int)PrivacyStatus.None,
                Sequence = 1,
                Title = $"Title for {_context.FullyQualifiedTestClassName} single MediaItem",
            };
        }

        private PlaylistModel CreateModelPlaylist()
        {
            var playlist = new PlaylistModel()
            {
                CreatedBy = _context.FullyQualifiedTestClassName,
                CreatedOn = DateTime.UtcNow,
                Description = $"Description for {_context.FullyQualifiedTestClassName} Playlist",
                Id = 1,
                IsDeleted = false,
                IsShuffeling = false,
                Location = "Memory",
                MediaItems = new List<MediaItemModel>(),
                PrivacyStatus = (int)PrivacyStatus.None,
                RepeatMode = (int)RepeatMode.None,
                Sequence = 1,
                Title = $"Title for {_context.FullyQualifiedTestClassName} Playlist",
                UpdatedBy = _context.FullyQualifiedTestClassName,
                UpdatedOn = DateTime.UtcNow,
            };

            return PopulatePlaylist(playlist);
        }

        private PlaylistModel PopulatePlaylist(PlaylistModel playlist)
        {
            for (var i = 0; i < 4; i++)
            {
                playlist.MediaItems.Add(new MediaItemModel()
                {
                    CreatedBy = _context.FullyQualifiedTestClassName,
                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow,
                    UpdatedBy = _context.FullyQualifiedTestClassName,
                    Description = $"Description for {_context.FullyQualifiedTestClassName} MediaItem number {i}",
                    Duration = 0,
                    Id = 1,
                    IsDeleted = false,
                    Location = "Memory",
                    Playlist = playlist,
                    PlaylistId = playlist.Id,
                    PrivacyStatus = (int)PrivacyStatus.None,
                    Sequence = 0,
                    Title = $"Title for {_context.FullyQualifiedTestClassName} MediaItem number {i}",
                });
            }

            return playlist;
        }

        private Playlist CreatePlaylist(PlaylistModel model)
        {
            var mapper = _container.Resolve<IPlaylistMapper>();
            return mapper.Get(model);
        }

        private MediaItem CreateMediaItem(MediaItemModel model)
        {
            var mapper = _container.Resolve<IMediaItemMapper>();
            return mapper.Get(model);
        }

        private static ViewModelServiceContainer CreateViewModelServiceContainer()
        {
            return new ViewModelServiceContainer(CreateLoggingService(), CreateILoggingNotifcationService(), CreateILocalizationService(), CreateIMessenger(), CreateISequenceService());
        }

        private static ISequenceService CreateISequenceService()
        {
            return Substitute.For<ISequenceService>();
        }

        private static IMessenger CreateIMessenger()
        {
            return Substitute.For<IMessenger>();
        }

        private static ILocalizationService CreateILocalizationService()
        {
            return Substitute.For<ILocalizationService>();
        }

        private static ILoggingNotifcationService CreateILoggingNotifcationService()
        {
            return Substitute.For<ILoggingNotifcationService>();
        }

        private static ILoggingService CreateLoggingService()
        {
            return Substitute.For<ILoggingService>();
        }

        private static IDialogViewModel CreateDialogViewModel()
        {
            return Substitute.For<IDialogViewModel>();
        }
    }
}
