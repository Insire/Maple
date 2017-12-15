using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        private static TestContext _context;
        private static ValidationResult _defaultValidationResult;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _context = context;

            _defaultValidationResult = new ValidationResult(Enumerable.Empty<ValidationFailure>());
        }

        [TestMethod]
        public async Task Playlist_ShouldRunConstructorWithoutErrors()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var model = CreateModelPlaylist();
            var playlist = CreatePlaylist(model, container);

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
        public async Task Playlist_ShouldThrowForEmptyModel()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            Assert.ThrowsException<ArgumentNullException>(() => CreatePlaylist(default(PlaylistModel), container));
        }

        [TestMethod]
        public async Task Playlist_ShouldThrowForEmptyContainer()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            Assert.ThrowsException<ArgumentNullException>(() => new Playlist(null, container.Resolve<IValidator<Playlist>>(), container.Resolve<IDialogViewModel>(), container.Resolve<IMediaItemMapper>(), CreateModelPlaylist()));
        }

        [TestMethod]
        public async Task Playlist_ShouldThrowForEmptyValidator()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            Assert.ThrowsException<ArgumentNullException>(() => new Playlist(container.Resolve<ViewModelServiceContainer>(), null, container.Resolve<IDialogViewModel>(), container.Resolve<IMediaItemMapper>(), CreateModelPlaylist()));
        }

        [TestMethod]
        public async Task Playlist_ShouldThrowForEmptyViewModel()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            Assert.ThrowsException<ArgumentNullException>(() => new Playlist(container.Resolve<ViewModelServiceContainer>(), container.Resolve<IValidator<Playlist>>(), null, container.Resolve<IMediaItemMapper>(), CreateModelPlaylist()));
        }

        [TestMethod]
        public async Task Playlist_ShouldThrowForEmptyMediaItemMapper()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            Assert.ThrowsException<ArgumentNullException>(() => new Playlist(container.Resolve<ViewModelServiceContainer>(), container.Resolve<IValidator<Playlist>>(), container.Resolve<IDialogViewModel>(), null, CreateModelPlaylist()));
        }

        [TestMethod]
        public async Task Playlist_ShouldRunClear()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var playlist = CreatePlaylist(CreateModelPlaylist(), container);

            Assert.AreEqual(4, playlist.Count);

            playlist.Clear();

            Assert.AreEqual(0, playlist.Count);
        }

        [TestMethod]
        public async Task Playlist_ShouldAdd()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var mediaItem = CreateMediaItem(CreateModelMediaItem(), container);
            var playlist = CreatePlaylist(CreateModelPlaylist(), container);

            Assert.AreEqual(4, playlist.Count);

            playlist.Add(mediaItem);

            Assert.AreEqual(5, playlist.Count);
        }

        [TestMethod]
        public async Task Playlist_ShouldThrowAddForNull()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var playlist = CreatePlaylist(CreateModelPlaylist(), container);

            Assert.ThrowsException<ArgumentNullException>(() => playlist.Add(null));
        }

        [TestMethod]
        public async Task Playlist_ShouldAddRange()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var mediaItems = new[]
            {
                CreateMediaItem(CreateModelMediaItem(), container),
                CreateMediaItem(CreateModelMediaItem(), container),
                CreateMediaItem(CreateModelMediaItem(), container),
            };
            var playlist = CreatePlaylist(CreateModelPlaylist(), container);

            Assert.AreEqual(4, playlist.Count);

            playlist.AddRange(mediaItems);

            Assert.AreEqual(7, playlist.Count);
        }

        [TestMethod]
        public async Task Playlist_ShouldHandleAddRangeForEmptyCollection()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var mediaItems = new List<MediaItem>();
            var playlist = CreatePlaylist(CreateModelPlaylist(), container);

            Assert.AreEqual(4, playlist.Count);

            playlist.AddRange(mediaItems);

            Assert.AreEqual(4, playlist.Count);
        }

        [TestMethod]
        public async Task Playlist_ShouldThrowAddRangeForNull()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var playlist = CreatePlaylist(CreateModelPlaylist(), container);

            Assert.AreEqual(4, playlist.Count);

            Assert.ThrowsException<ArgumentNullException>(() => playlist.AddRange(null));
        }

        [TestMethod]
        public async Task Playlist_ShouldHandleAddRangeForDuplicateEntries()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var mediaItem = CreateMediaItem(CreateModelMediaItem(), container);
            var mediaItems = new[]
            {
                mediaItem,
                mediaItem,
                mediaItem,
            };
            var playlist = CreatePlaylist(CreateModelPlaylist(), container);

            Assert.AreEqual(4, playlist.Count);

            playlist.AddRange(mediaItems);

            Assert.AreEqual(7, playlist.Count);
        }

        [TestMethod]
        public async Task Playlist_ShouldRemove()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var playlist = CreatePlaylist(CreateModelPlaylist(), container);

            Assert.AreEqual(4, playlist.Count);

            playlist.Remove(playlist[0]);

            Assert.AreEqual(3, playlist.Count);
        }

        [TestMethod]
        public async Task Playlist_ShouldThrowRemoveForNull()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var playlist = CreatePlaylist(CreateModelPlaylist(), container);

            Assert.ThrowsException<ArgumentNullException>(() => playlist.Remove(null));
        }

        [TestMethod]
        public async Task Playlist_ShouldRemoveRange()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var playlist = CreatePlaylist(CreateModelPlaylist(), container);
            var mediaItems = new[]
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
        public async Task Playlist_ShouldThrowRemoveRangeForNull()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var playlist = CreatePlaylist(CreateModelPlaylist(), container);

            Assert.ThrowsException<ArgumentNullException>(() => playlist.RemoveRange(null));
        }

        [TestMethod]
        public async Task Playlist_ShouldHandleRemoveRangeForSameItem()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var playlist = CreatePlaylist(CreateModelPlaylist(), container);
            var mediaItems = new[]
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
        public async Task Playlist_ShouldHandleRemoveRangeForUnknownItem()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var playlist = CreatePlaylist(CreateModelPlaylist(), container);
            var mediaItems = new List<MediaItem>()
            {
                CreateMediaItem(new MediaItemModel(), container),
            };

            Assert.AreEqual(4, playlist.Count);

            playlist.RemoveRange(mediaItems);

            Assert.AreEqual(4, playlist.Count);
        }

        [TestMethod]
        public async Task Playlist_ShouldHandleRemoveRangeForUnknownItems()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var playlist = CreatePlaylist(CreateModelPlaylist(), container);
            var mediaItems = new[]
            {
                CreateMediaItem(new MediaItemModel(), container),
                CreateMediaItem(new MediaItemModel(), container),
                CreateMediaItem(new MediaItemModel(), container),
            };

            Assert.AreEqual(4, playlist.Count);

            playlist.RemoveRange(mediaItems);

            Assert.AreEqual(4, playlist.Count);
        }

        [TestMethod]
        public async Task Playlist_ShouldHandleRemoveRangeForEmptyCollection()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var playlist = CreatePlaylist(CreateModelPlaylist(), container);
            var mediaItems = new List<MediaItem>();

            Assert.AreEqual(4, playlist.Count);

            playlist.RemoveRange(mediaItems);

            Assert.AreEqual(4, playlist.Count);
        }

        [TestMethod]
        public async Task Playlist_ShouldRunNext()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var playlist = CreatePlaylist(CreateModelPlaylist(), container);
            playlist.RepeatMode = RepeatMode.None;
            var mediaItem = playlist.Next();

            Assert.IsNotNull(mediaItem);
            Assert.AreEqual(playlist[1], mediaItem);
            Assert.AreNotEqual(playlist.SelectedItem, mediaItem);
        }

        [TestMethod]
        public async Task Playlist_ShouldRunNextWithRepeatModeNone()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var playlist = CreatePlaylist(CreateModelPlaylist(), container);
            playlist.RepeatMode = RepeatMode.None;
            playlist.SelectedItem = playlist[3];

            var mediaItem = playlist.Next();

            Assert.IsNull(mediaItem);
        }

        [TestMethod]
        public async Task Playlist_ShouldRunNextWithRepeatModeAll()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var playlist = CreatePlaylist(CreateModelPlaylist(), container);
            playlist.RepeatMode = RepeatMode.All;
            playlist.SelectedItem = playlist[3];

            var mediaItem = playlist.Next();

            Assert.IsNotNull(mediaItem);
            Assert.AreEqual(playlist[0], mediaItem);
        }

        [TestMethod]
        public async Task Playlist_ShouldRunNextWithRepeatModeAllWhileShuffeling()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var playlist = CreatePlaylist(CreateModelPlaylist(), container);
            playlist.RepeatMode = RepeatMode.All;
            playlist.IsShuffeling = true;
            playlist.SelectedItem = playlist[3];

            var mediaItem = playlist.Next();

            Assert.IsNotNull(mediaItem);
            Assert.AreNotEqual(playlist.SelectedItem, mediaItem);
        }

        [TestMethod]
        public async Task Playlist_ShouldRunNextWithRepeatModeNoneWhileShuffeling()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var playlist = CreatePlaylist(CreateModelPlaylist(), container);
            playlist.RepeatMode = RepeatMode.None;
            playlist.IsShuffeling = true;
            playlist.SelectedItem = playlist[3];

            var mediaItem = playlist.Next();

            Assert.IsNotNull(mediaItem);
            Assert.AreNotEqual(playlist.SelectedItem, mediaItem);
        }

        [TestMethod]
        public async Task Playlist_ShouldRunNextWithRepeatModeSingleWhileShuffeling()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var playlist = CreatePlaylist(CreateModelPlaylist(), container);
            playlist.RepeatMode = RepeatMode.Single;
            playlist.IsShuffeling = true;

            var mediaItem = playlist.Next();

            Assert.IsNotNull(mediaItem);
            Assert.AreEqual(playlist[0], mediaItem);
            Assert.AreEqual(playlist.SelectedItem, mediaItem);
        }

        [TestMethod]
        public async Task Playlist_ShouldRunNextWithRepeatModeSingle()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var playlist = CreatePlaylist(CreateModelPlaylist(), container);
            playlist.RepeatMode = RepeatMode.Single;

            var mediaItem = playlist.Next();

            Assert.IsNotNull(mediaItem);
            Assert.AreEqual(playlist[0], mediaItem);
            Assert.AreEqual(playlist.SelectedItem, mediaItem);
        }

        [TestMethod]
        public async Task Playlist_ShouldRunPrevious()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var playlist = CreatePlaylist(CreateModelPlaylist(), container);
            var messenger = container.Resolve<IMessenger>();

            messenger.Publish(new PlayingMediaItemMessage(this, playlist[0], playlist.Id));
            messenger.Publish(new PlayingMediaItemMessage(this, playlist[1], playlist.Id));
            messenger.Publish(new PlayingMediaItemMessage(this, playlist[2], playlist.Id));

            var previous = playlist.Previous();
            Assert.AreEqual(playlist[2], previous);
            Assert.AreEqual(true, previous.IsSelected);
            Assert.AreEqual(1, playlist.Items.Where(p => p.IsSelected).Count());

            previous = playlist.Previous();
            Assert.AreEqual(playlist[1], previous);
            Assert.AreEqual(true, previous.IsSelected);
            Assert.AreEqual(1, playlist.Items.Where(p => p.IsSelected).Count());

            previous = playlist.Previous();
            Assert.AreEqual(playlist[0], previous);
            Assert.AreEqual(true, previous.IsSelected);
            Assert.AreEqual(1, playlist.Items.Where(p => p.IsSelected).Count());

            previous = playlist.Previous();
            Assert.AreEqual(null, previous);
            Assert.AreEqual(0, playlist.Items.Where(p => p.IsSelected).Count());

            previous = playlist.Previous();
            Assert.AreEqual(null, previous);
            Assert.AreEqual(0, playlist.Items.Where(p => p.IsSelected).Count());
        }

        [TestMethod]
        public async Task Playlist_ShouldRaiseSelectionChanging()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var messenger = Substitute.For<IMessenger>();
            container.UseInstance(typeof(IMessenger), messenger, IfAlreadyRegistered: IfAlreadyRegistered.Replace);

            Assert.AreEqual(messenger, container.Resolve<IMessenger>());

            var playlist = CreatePlaylist(CreateModelPlaylist(), container);

            messenger.ClearReceivedCalls();
            playlist.SelectedItem = playlist[1];

            messenger.Received(1).Publish(NSubstitute.Arg.Any<ViewModelSelectionChangingMessage<MediaItem>>());
        }

        [TestMethod]
        public async Task Playlist_ShouldRaiseSelectionChanged()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var messenger = Substitute.For<IMessenger>();
            container.UseInstance(typeof(IMessenger), messenger, IfAlreadyRegistered: IfAlreadyRegistered.Replace);

            Assert.AreEqual(messenger, container.Resolve<IMessenger>());

            var playlist = CreatePlaylist(CreateModelPlaylist(), container);

            messenger.ClearReceivedCalls();
            playlist.SelectedItem = playlist[1];

            messenger.Received(1).Publish(NSubstitute.Arg.Any<ViewModelSelectionChangedMessage<MediaItem>>());
        }

        [TestMethod]
        public async Task Playlist_ShouldSynchronizeItemsWithModelWhenRemovingSelectedItem()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var model = CreateModelPlaylist();
            var playlist = CreatePlaylist(model, container);
            var selectedModel = playlist.SelectedItem.Model;
            var next = playlist.Next();

            Assert.AreEqual(model.MediaItems.Count, playlist.Count);

            playlist.Remove(playlist.SelectedItem);

            Assert.AreEqual(3, playlist.Count);
            Assert.AreEqual(true, selectedModel.IsDeleted);
            Assert.AreEqual(next, playlist.SelectedItem);
        }

        [TestMethod]
        public async Task Playlist_ShouldSynchronizeItemsWithModelWhenRemoving()
        {
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var model = CreateModelPlaylist();
            var playlist = CreatePlaylist(model, container);
            var mediaItem1 = playlist[1];
            var mediaItem2 = playlist[2];
            var mediaItem3 = playlist[3];

            Assert.AreEqual(model.MediaItems.Count, playlist.Count);

            playlist.Remove(mediaItem1);

            Assert.AreEqual(3, playlist.Count);
            Assert.AreEqual(true, mediaItem1.IsDeleted);

            playlist.RemoveRange(new[] { mediaItem2, mediaItem3 });

            Assert.AreEqual(1, playlist.Count);
            Assert.AreEqual(true, mediaItem2.IsDeleted);
            Assert.AreEqual(true, mediaItem3.IsDeleted);
        }

        [TestMethod]
        public async Task Playlist_ShouldAddItemsFromFileDialog()
        {
            var tokenSource = new CancellationTokenSource(1000);
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var mediaItems = new[]
{
                CreateMediaItem(new MediaItemModel(), container),
                CreateMediaItem(new MediaItemModel(), container),
                CreateMediaItem(new MediaItemModel(), container),
            };
            var dialogViewModel = Substitute.For<IDialogViewModel>();
            dialogViewModel.ShowMediaItemSelectionDialog(NSubstitute.Arg.Any<FileSystemBrowserOptions>(), NSubstitute.Arg.Any<CancellationToken>()).Returns((true, mediaItems));
            container.UseInstance(typeof(IDialogViewModel), dialogViewModel, IfAlreadyRegistered: IfAlreadyRegistered.Replace);

            var playlist = CreatePlaylist(CreateModelPlaylist(), container);

            foreach (var item in mediaItems)
                Assert.AreEqual(false, playlist.Items.Contains(item));

            await playlist.LoadFromFileCommand.ExecuteAsync(tokenSource.Token).ConfigureAwait(false);

            foreach (var item in mediaItems)
                Assert.AreEqual(true, playlist.Items.Contains(item));
        }

        [TestMethod]
        public async Task Playlist_ShouldAddItemsFromFolderDialog()
        {
            var tokenSource = new CancellationTokenSource(1000);
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var mediaItems = new[]
{
                CreateMediaItem(new MediaItemModel(), container),
                CreateMediaItem(new MediaItemModel(), container),
                CreateMediaItem(new MediaItemModel(), container),
            };
            var dialogViewModel = Substitute.For<IDialogViewModel>();
            dialogViewModel.ShowMediaItemFolderSelectionDialog(NSubstitute.Arg.Any<FileSystemFolderBrowserOptions>(), NSubstitute.Arg.Any<CancellationToken>()).Returns((true, mediaItems));
            container.UseInstance(typeof(IDialogViewModel), dialogViewModel, IfAlreadyRegistered: IfAlreadyRegistered.Replace);

            var playlist = CreatePlaylist(CreateModelPlaylist(), container);

            foreach (var item in mediaItems)
                Assert.AreEqual(false, playlist.Items.Contains(item));

            await playlist.LoadFromFolderCommand.ExecuteAsync(tokenSource.Token).ConfigureAwait(false);

            foreach (var item in mediaItems)
                Assert.AreEqual(true, playlist.Items.Contains(item));
        }

        [TestMethod]
        public async Task Playlist_ShouldAddItemsFromUrlDialog()
        {
            var tokenSource = new CancellationTokenSource(1000);
            var container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            var mediaItems = new[]
{
                CreateMediaItem(new MediaItemModel(), container),
                CreateMediaItem(new MediaItemModel(), container),
                CreateMediaItem(new MediaItemModel(), container),
            };
            var dialogViewModel = Substitute.For<IDialogViewModel>();
            dialogViewModel.ShowUrlParseDialog(NSubstitute.Arg.Any<CancellationToken>()).Returns((mediaItems));
            container.UseInstance(typeof(IDialogViewModel), dialogViewModel, IfAlreadyRegistered: IfAlreadyRegistered.Replace);

            var playlist = CreatePlaylist(CreateModelPlaylist(), container);

            foreach (var item in mediaItems)
                Assert.AreEqual(false, playlist.Items.Contains(item));

            await playlist.LoadFromUrlCommand.ExecuteAsync(tokenSource.Token).ConfigureAwait(false);

            foreach (var item in mediaItems)
                Assert.AreEqual(true, playlist.Items.Contains(item));
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

        private Playlist CreatePlaylist(PlaylistModel model, IContainer container)
        {
            var mapper = container.Resolve<IPlaylistMapper>();
            return mapper.Get(model);
        }

        private MediaItem CreateMediaItem(MediaItemModel model, IContainer container)
        {
            var mapper = container.Resolve<IMediaItemMapper>();
            return mapper.Get(model);
        }

        private static ViewModelServiceContainer CreateViewModelServiceContainer(IContainer container)
        {
            return new ViewModelServiceContainer(CreateLoggingService(), CreateILoggingNotifcationService(), CreateILocalizationService(), container.Resolve<IMessenger>(), CreateISequenceService());
        }

        private static ISequenceService CreateISequenceService()
        {
            return Substitute.For<ISequenceService>();
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
