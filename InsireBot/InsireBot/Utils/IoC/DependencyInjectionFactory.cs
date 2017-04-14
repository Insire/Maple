using DryIoc;
using Maple.Core;
using Maple.Youtube;

namespace Maple
{
    /// <summary>
    /// Factory class that provides an Instance of <see cref="IContainer"/>
    /// </summary>
    public class DependencyInjectionFactory
    {
        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <returns></returns>
        public static IContainer GetContainer()
        {
            var container = new Container(rules => rules.WithoutThrowOnRegisteringDisposableTransient());

            RegisterViewModels();
            RegisterServices();

            container.Register<Scenes>(Reuse.Singleton);
            container.Resolve<ITranslationService>().Load();

            return container;

            void RegisterViewModels()
            {
                // save-/loadable ViewModels
                container.Register<Playlists>(Reuse.Singleton);
                container.Register<MediaPlayers>(Reuse.Singleton);
                container.Register<OptionsViewModel>(Reuse.Singleton);
                container.Register<UIColorsViewModel>(Reuse.Singleton);
                container.Register<SplashScreenViewModel>(Reuse.Singleton);

                // generic ViewModels
                container.Register<AudioDevices>(Reuse.Singleton);
                container.Register<ShellViewModel>(Reuse.Singleton);
                container.Register<DialogViewModel>(Reuse.Singleton);
                container.Register<StatusbarViewModel>(Reuse.Singleton);

                // "Overloads" for already registered ViewModels, so i can query them via the container by resolving the specified interface
                container.RegisterMapping<ILoadableViewModel, Playlists>(Reuse.Singleton);
                container.RegisterMapping<ILoadableViewModel, MediaPlayers>(Reuse.Singleton);
                container.RegisterMapping<ILoadableViewModel, OptionsViewModel>(Reuse.Singleton);
                container.RegisterMapping<ILoadableViewModel, UIColorsViewModel>(Reuse.Singleton);

                // Decorator for logging Loading and Saving
                container.Register<ILoadableViewModel, RefreshableDecorator>(setup: Setup.Decorator);
            };

            void RegisterServices()
            {
                // misc
                container.Register<IMediaPlayer, NAudioMediaPlayer>(Reuse.Transient);
                container.Register<IMediaRepository, MediaRepository>();
                container.Register<IMediaItemMapper, MediaItemMapper>();
                container.Register<IPlaylistMapper, PlaylistMapper>();
                container.Register<IMapleLog, LoggingService>(Reuse.Singleton);
                container.Register<IYoutubeUrlParseService, UrlParseService>();
                container.Register<ITranslationProvider, ResxTranslationProvider>(Reuse.Singleton);
                container.Register<ITranslationService, TranslationService>(Reuse.Singleton);
                container.Register<ISequenceProvider, SequenceService>();
                container.Register<FileSystemViewModel>();
            }
        }
    }
}
