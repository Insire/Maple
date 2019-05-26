using System.Threading.Tasks;
using DryIoc;
using FluentValidation;
using Maple.Core;
using Maple.Data;
using Maple.Domain;
using Maple.Log;
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

                c.Resolve<ILocalizationService>().Load();

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
                c.RegisterMany(new[] { typeof(IPlaylistsViewModel) }, typeof(Playlists), Reuse.Singleton);
                c.RegisterMany(new[] { typeof(IMediaPlayersViewModel) }, typeof(MediaPlayers), Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));
                c.RegisterMany(new[] { typeof(ICultureViewModel) }, typeof(Cultures), Reuse.Singleton);
                c.RegisterMany(new[] { typeof(IUIColorsViewModel) }, typeof(UIColorsViewModel), Reuse.Singleton);

                //generic ViewModels
                c.Register<Scenes>(Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));
                c.Register<AudioDevices>(Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));
                c.Register<ShellViewModel>(Reuse.Singleton);
                c.Register<IDialogViewModel, DialogViewModel>(Reuse.Singleton);
                c.Register<StatusbarViewModel>(Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));
                c.Register<FileSystemViewModel>(Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));
                c.Register<OptionsViewModel>(Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));

                c.Register<CreateMediaItem>(setup: Setup.With(allowDisposableTransient: true));
                c.Register<CreatePlaylist>(setup: Setup.With(allowDisposableTransient: true));

                c.Register<ISplashScreenViewModel, SplashScreenViewModel>(Reuse.Singleton);
            };

            void RegisterServices()
            {
                c.Register<IMediaPlayer, NAudioMediaPlayer>(setup: Setup.With(allowDisposableTransient: true));
                c.Register<IWavePlayerFactory, WavePlayerFactory>(Reuse.Singleton);

                c.Register<PlaylistContext>(Reuse.Transient, setup: Setup.With(allowDisposableTransient: true));

                c.Register<IPlaylistRepository, PlaylistRepository>(Reuse.Transient, setup: Setup.With(allowDisposableTransient: true));
                c.Register<IMediaItemRepository, MediaItemRepository>(Reuse.Transient, setup: Setup.With(allowDisposableTransient: true));
                c.Register<IMediaPlayerRepository, MediaPlayerRepository>(Reuse.Transient, setup: Setup.With(allowDisposableTransient: true));

                c.Register<IUnitOfWork, MapleUnitOfWork>(Reuse.Transient, setup: Setup.With(allowDisposableTransient: true));

                c.Register<ISequenceService, SequenceService>();
                c.Register<IYoutubeUrlParser, YoutubeUrlParser>();

                c.Register<IVersionService, VersionService>(Reuse.Singleton);
                c.Register<ILocalizationService, LocalizationServiceBase>(Reuse.Singleton);
                c.Register<ITranslationProvider, ResxTranslationProvider>(Reuse.Singleton);

                c.Register<IMessenger, ScarletMessenger>(Reuse.Singleton);
                c.Register<IScarletMessageProxy, DefaultMessageProxy>(Reuse.Singleton);

                c.Register<ILoggingNotifcationService, LoggingNotifcationService>(Reuse.Singleton);
                c.Register<ILoggingService, LoggingService>(Reuse.Singleton);
                c.Register<ILoggingService, DetailLoggingService>(setup: Setup.DecoratorWith(order: 1));
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
