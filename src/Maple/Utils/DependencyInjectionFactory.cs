using System.Threading.Tasks;
using DryIoc;
using FluentValidation;
using Maple.Core;
using Maple.Data;
using Maple.Domain;
using Maple.Youtube;
using Microsoft.Extensions.Logging;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Abstractions;
using MvvmScarletToolkit.FileSystemBrowser;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    /// <summary>
    /// Factory class that provides an Instance of <see cref="IContainer"/>
    /// </summary>
    public static class DependencyInjectionFactory
    {
        public static Task<IContainer> Get()
        {
            var c = new Container();
            return Task.Run(() => InitializeContainer());

            IContainer InitializeContainer()
            {
                RegisterViewModels();
                RegisterServices();
                RegisterValidation();
                RegisterControls();

                return c;
            }

            void RegisterControls()
            {
                c.Register<Shell>();
                c.Register<SplashScreen>();
            }

            void RegisterViewModels()
            {
                // TODO register disposeables

                // save-/loadable ViewModels
                c.RegisterMany(new[] { typeof(Playlists) }, typeof(Playlists), Reuse.Singleton);
                c.RegisterMany(new[] { typeof(MediaPlayers) }, typeof(MediaPlayers), Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));
                c.RegisterMany(new[] { typeof(ICultureViewModel) }, typeof(Cultures), Reuse.Singleton);
                c.RegisterMany(new[] { typeof(IUIColorsViewModel) }, typeof(UIColorsViewModel), Reuse.Singleton);

                //generic ViewModels
                c.Register<Scenes>(Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));
                c.Register<AudioDevices>(Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));
                c.Register<ShellViewModel>(Reuse.Singleton);
                c.Register<DialogViewModel>(Reuse.Singleton);
                c.Register<StatusbarViewModel>(Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));
                c.Register<FileSystemViewModel>(Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));
                c.Register<OptionsViewModel>(Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));

                c.Register<YoutubeImportViewModel>(setup: Setup.With(allowDisposableTransient: true));
                c.Register<YoutubeImportViewModel>(setup: Setup.With(allowDisposableTransient: true));

                c.Register<ISplashScreenViewModel, SplashScreenViewModel>(Reuse.Singleton);
            };

            void RegisterServices()
            {
                c.Register<PlaylistContext>(Reuse.Transient, setup: Setup.With(allowDisposableTransient: true));

                c.Register<IPlaylistRepository, PlaylistRepository>(Reuse.Transient, setup: Setup.With(allowDisposableTransient: true));
                c.Register<IMediaItemRepository, MediaItemRepository>(Reuse.Transient, setup: Setup.With(allowDisposableTransient: true));
                c.Register<IMediaPlayerRepository, MediaPlayerRepository>(Reuse.Transient, setup: Setup.With(allowDisposableTransient: true));

                c.Register<IUnitOfWork, MapleUnitOfWork>(Reuse.Transient, setup: Setup.With(allowDisposableTransient: true));

                c.Register<ISequenceService, SequenceService>();
                c.Register<IYoutubeService, YoutubeService>();

                c.Register<IVersionService, VersionService>(Reuse.Singleton);
                c.Register<ILocalizationService, LocalizationServiceBase>(Reuse.Singleton);
                c.Register<ILocalizationProvider, ResxTranslationProvider>(Reuse.Singleton);

                c.Register<IMessenger, ScarletMessenger>(Reuse.Singleton);
                c.Register<IScarletMessageProxy, DefaultMessageProxy>(Reuse.Singleton);

                c.Register<ILoggingNotifcationService, LoggingNotifcationService>(Reuse.Singleton);
                c.Register<ILoggingService, SquirrelLogger>(setup: Setup.DecoratorWith(order: 2));
                c.Register<ILoggerFactory, LoggerFactory>(Reuse.Singleton, Made.Of(() => new LoggerFactory()));
            }

            void RegisterValidation()
            {
                c.Register<IValidator<Playlist>, PlaylistValidator>(Reuse.Singleton);
                c.Register<IValidator<Playlists>, PlaylistsValidator>(Reuse.Singleton);
                c.Register<IValidator<MediaPlayer>, MediaPlayerValidator>(Reuse.Singleton);
                c.Register<IValidator<MediaItem>, MediaItemValidator>(Reuse.Singleton);
            }
        }
    }
}
