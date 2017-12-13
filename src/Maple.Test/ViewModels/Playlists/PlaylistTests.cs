using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DryIoc;
using FluentValidation;
using FluentValidation.Results;
using Maple.Core;
using Maple.Interfaces;
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
        public void ShouldRunConstructorWithoutErrors()
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
        public void ShouldThrowForEmptyModel()
        {
            Assert.ThrowsException<ArgumentNullException>(() => CreatePlaylist(default(Data.Playlist)));
        }

        [TestMethod]
        public void ShouldThrowForEmptyContainer()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new Playlist(null, _container.Resolve<IValidator<Playlist>>(), _container.Resolve<IDialogViewModel>(), _container.Resolve<IMediaItemMapper>(), CreateModelPlaylist()));
        }

        [TestMethod]
        public void ShouldThrowForEmptyValidator()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new Playlist(_container.Resolve<ViewModelServiceContainer>(), null, _container.Resolve<IDialogViewModel>(), _container.Resolve<IMediaItemMapper>(), CreateModelPlaylist()));
        }

        [TestMethod]
        public void ShouldThrowForEmptyViewModel()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new Playlist(_container.Resolve<ViewModelServiceContainer>(), _container.Resolve<IValidator<Playlist>>(), null, _container.Resolve<IMediaItemMapper>(), CreateModelPlaylist()));
        }

        [TestMethod]
        public void ShouldThrowForEmptyMediaItemMapper()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new Playlist(_container.Resolve<ViewModelServiceContainer>(), _container.Resolve<IValidator<Playlist>>(), _container.Resolve<IDialogViewModel>(), null, CreateModelPlaylist()));
        }

        [TestMethod]
        public void ShouldRunClear()
        {
            var playlist = CreatePlaylist(CreateModelPlaylist());

            Assert.AreEqual(4, playlist.Count);

            playlist.Clear();

            Assert.AreEqual(0, playlist.Count);
        }

        [TestMethod]
        public void ShouldAdd()
        {
            var mediaItem = CreateMediaItem(CreateModelMediaItem());
            var playlist = CreatePlaylist(CreateModelPlaylist());

            Assert.AreEqual(4, playlist.Count);

            playlist.Add(mediaItem);

            Assert.AreEqual(5, playlist.Count);
        }

        [TestMethod]
        public void ShouldAddRanage()
        {
            throw new NotImplementedException();
            CreatePlaylist(CreateModelPlaylist());
        }

        [TestMethod]
        public void ShouldRemove()
        {
            throw new NotImplementedException();
            CreatePlaylist(CreateModelPlaylist());
        }

        [TestMethod]
        public void ShouldRemoveRange()
        {
            throw new NotImplementedException();
            CreatePlaylist(CreateModelPlaylist());
        }

        [TestMethod]
        public void ShouldRunNext()
        {
            throw new NotImplementedException();
            CreatePlaylist(CreateModelPlaylist());
        }

        [TestMethod]
        public void ShouldRunPrevious()
        {
            throw new NotImplementedException();
            CreatePlaylist(CreateModelPlaylist());
        }

        [TestMethod]
        public void ShouldRaiseSelectionChanging()
        {
            throw new NotImplementedException();
            CreatePlaylist(CreateModelPlaylist());
        }

        [TestMethod]
        public void ShouldRaiseSelectionChanged()
        {
            throw new NotImplementedException();
            CreatePlaylist(CreateModelPlaylist());
        }

        private Data.MediaItem CreateModelMediaItem()
        {
            return new Data.MediaItem()
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

        private Data.Playlist CreateModelPlaylist()
        {
            var playlist = new Data.Playlist()
            {
                CreatedBy = _context.FullyQualifiedTestClassName,
                CreatedOn = DateTime.UtcNow,
                Description = $"Description for {_context.FullyQualifiedTestClassName} Playlist",
                Id = 1,
                IsDeleted = false,
                IsShuffeling = false,
                Location = "Memory",
                MediaItems = new List<Data.MediaItem>(),
                PrivacyStatus = (int)PrivacyStatus.None,
                RepeatMode = (int)RepeatMode.None,
                Sequence = 1,
                Title = $"Title for {_context.FullyQualifiedTestClassName} Playlist",
                UpdatedBy = _context.FullyQualifiedTestClassName,
                UpdatedOn = DateTime.UtcNow,
            };

            return PopulatePlaylist(playlist);
        }

        private Data.Playlist PopulatePlaylist(Data.Playlist playlist)
        {
            for (var i = 0; i < 4; i++)
            {
                playlist.MediaItems.Add(new Data.MediaItem()
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
                    Sequence = 1,
                    Title = $"Title for {_context.FullyQualifiedTestClassName} MediaItem number {i}",
                });
            }

            return playlist;
        }

        private Playlist CreatePlaylist(Data.Playlist model)
        {
            var mapper = _container.Resolve<IPlaylistMapper>();
            return mapper.Get(model);
        }

        private MediaItem CreateMediaItem(Data.MediaItem model)
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
