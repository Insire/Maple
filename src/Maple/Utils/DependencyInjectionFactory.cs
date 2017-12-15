using System.Threading.Tasks;
using DryIoc;
using FluentValidation;
using Maple.Core;
using Maple.Data;
using Maple.Domain;
using Maple.Youtube;

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

                c.Resolve<ILocalizationService>().LoadAsync();

                //if (Debugger.IsAttached)
                //    Debugging();

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
                c.RegisterMany(new[] { typeof(ILoadableViewModel), typeof(IPlaylistsViewModel) }, typeof(Playlists), Reuse.Singleton);
                c.RegisterMany(new[] { typeof(ILoadableViewModel), typeof(IMediaPlayersViewModel) }, typeof(MediaPlayers), Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));
                c.RegisterMany(new[] { typeof(ILoadableViewModel), typeof(ICultureViewModel) }, typeof(Cultures), Reuse.Singleton);
                c.RegisterMany(new[] { typeof(ILoadableViewModel), typeof(IUIColorsViewModel) }, typeof(UIColorsViewModel), Reuse.Singleton);

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
                c.Register<IMediaRepository, MediaRepository>(Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));

                c.Register<ViewModelServiceContainer>(Reuse.Singleton);
                c.Register<PlaylistContext>(Reuse.Transient, setup: Setup.With(allowDisposableTransient: true));

                c.Register<IPlaylistRepository, PlaylistRepository>(Reuse.Singleton);
                c.Register<IMediaItemRepository, MediaItemRepository>(Reuse.Singleton);
                c.Register<IMediaPlayerRepository, MediaPlayerRepository>(Reuse.Singleton);

                c.Register<IPlaylistMapper, PlaylistMapper>();
                c.Register<IMediaPlayerMapper, MediaPlayerMapper>();
                c.Register<IMediaItemMapper, MediaItemMapper>();
                c.Register<ISequenceService, SequenceService>();
                c.Register<IYoutubeUrlParser, YoutubeUrlParser>();

                c.Register<IVersionService, VersionService>(Reuse.Singleton);
                c.Register<ILocalizationService, LocalizationService>(Reuse.Singleton);
                c.Register<ITranslationProvider, ResxTranslationProvider>(Reuse.Singleton);

                c.Register<IMessenger, MapleMessenger>(Reuse.Singleton);
                c.Register<IMapleMessageProxy, DefaultMessageProxy>(Reuse.Singleton);

                c.Register<ILoggingNotifcationService, LoggingNotifcationService>(Reuse.Singleton);
                c.Register<ILoggingService, LoggingService>(Reuse.Singleton);
                c.Register<ILoggingService, DetailLoggingService>(setup: Setup.Decorator);
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
