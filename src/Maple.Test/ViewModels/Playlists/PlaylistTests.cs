using System.Threading.Tasks;
using FluentValidation;
using Maple;
using Maple.Core;
using Maple.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using DryIoc;

namespace Maple.Test.ViewModels.Playlists
{
    [TestClass]
    public class PlaylistTests
    {
        private static IContainer _container;

        [ClassInitialize]
        public static async Task TestInitialize()
        {
            _container = await DependencyInjectionFactory.Get().ConfigureAwait(false);
            _container.UseInstance(CreateViewModelServiceContainer());
            _container.UseInstance(CreateLoggingService());
            _container.UseInstance(CreateILoggingNotifcationService());
            _container.UseInstance(CreateILocalizationService());
            _container.UseInstance(CreateIMessenger());
            _container.UseInstance(CreateISequenceService());

            _container.UseInstance(CreateValidator());
            _container.UseInstance(CreateDialogViewModel());
        }

        [TestMethod]
        public void ShouldRunConstructor()
        {
            // Arrange


            // Act
            var playlist = CreatePlaylist(CreateModel());


            // Assert

        }

        private Data.Playlist CreateModel()
        {
            return new Data.Playlist();
        }

        private Playlist CreatePlaylist(Data.Playlist model)
        {
            return new Playlist(
                CreateViewModelServiceContainer(),
                CreateValidator(),
                CreateDialogViewModel(),
                model);
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

        private static IValidator<Playlist> CreateValidator()
        {
            return Substitute.For<IValidator<Playlist>>();
        }

        private static IDialogViewModel CreateDialogViewModel()
        {
            return Substitute.For<IDialogViewModel>();
        }
    }
}
