using DryIoc;
using Maple.Core;
using Maple.Domain;
using NSubstitute;

namespace Maple.Test
{
    public static class ContainerContextExtensions
    {
        public static IContainer ConfigureForTesting(this IContainer container)
        {
            container.UseInstance(CreatePlaybackDeviceFactory());
            container.UseInstance(CreateWavePlayerFactory());

            return container;
        }

        public static Playlists CreatePlaylists(this IContainer container)
        {
            return new Playlists(container.CreateViewModelServiceContainer(), container.Resolve<IPlaylistMapper>(), () => container.Resolve<IMediaRepository>());
        }

        public static ViewModelServiceContainer CreateViewModelServiceContainer(this IContainer container)
        {
            return new ViewModelServiceContainer(CreateLoggingService(), CreateILoggingNotifcationService(), CreateILocalizationService(), container.Resolve<IMessenger>(), CreateSequenceService());
        }

        public static Playlist CreatePlaylist(this IContainer container, PlaylistModel model)
        {
            var mapper = container.Resolve<IPlaylistMapper>();
            return mapper.Get(model);
        }

        public static MediaItem CreateMediaItem(this IContainer container, MediaItemModel model)
        {
            var mapper = container.Resolve<IMediaItemMapper>();
            return mapper.Get(model);
        }

        public static IPlaybackDeviceFactory CreatePlaybackDeviceFactory()
        {
            var factory = Substitute.For<IPlaybackDeviceFactory>();

            factory.GetAudioDevices(default(ILoggingService)).ReturnsForAnyArgs(
                new[]
                {
                    new AudioDevice()
                    {
                        Channels= 2,
                        IsSelected = true,
                        Name = "TestDevice",
                        Sequence = 1,
                    }
                });

            return factory;
        }

        // should not be used as default replacement in the container, since some tests rely on the messenger relaying message in the class to be tested
        public static IMessenger CreateMessenger()
        {
            return Substitute.For<IMessenger>();
        }

        public static IWavePlayerFactory CreateWavePlayerFactory()
        {
            return Substitute.For<IWavePlayerFactory>();
        }

        public static ISequenceService CreateSequenceService()
        {
            return Substitute.For<ISequenceService>();
        }

        public static ILocalizationService CreateILocalizationService()
        {
            return Substitute.For<ILocalizationService>();
        }

        public static ILoggingNotifcationService CreateILoggingNotifcationService()
        {
            return Substitute.For<ILoggingNotifcationService>();
        }

        public static ILoggingService CreateLoggingService()
        {
            return Substitute.For<ILoggingService>();
        }

        public static IDialogViewModel CreateDialogViewModel()
        {
            return Substitute.For<IDialogViewModel>();
        }

        public static IMediaRepository CreateRepository()
        {
            return Substitute.For<IMediaRepository>();
        }
    }
}
