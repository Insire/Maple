using DryIoc;

using Maple.Core;
using Maple.Domain;

using NSubstitute;

namespace Maple.Test
{
    public static class ContainerContextExtensions
    {
        public static IUnitOfWork CreateRepository()
        {
            return Substitute.For<IUnitOfWork>();
        }

        public static Playlists CreatePlaylists(this IContainer container)
        {
            return new Playlists(container.CreateViewModelServiceContainer(), container.Resolve<IPlaylistMapper>(), () => container.Resolve<IUnitOfWork>());
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
    }
}
