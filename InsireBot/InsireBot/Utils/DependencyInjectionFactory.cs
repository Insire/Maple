using DryIoc;
using FluentValidation;
using Maple.Core;
using Maple.Data;
using Maple.Youtube;
using System.Threading.Tasks;

namespace Maple
{
    /// <summary>
    /// Factory class that provides an Instance of <see cref="IContainer"/>
    /// </summary>
    public class DependencyInjectionFactory
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
                // save-/loadable ViewModels
                c.RegisterMany(new[] { typeof(ILoadableViewModel), typeof(IPlaylistsViewModel) }, typeof(Playlists), Reuse.Singleton);
                c.RegisterMany(new[] { typeof(ILoadableViewModel), typeof(IMediaPlayersViewModel) }, typeof(MediaPlayers), Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));
                c.RegisterMany(new[] { typeof(ILoadableViewModel), typeof(ICultureViewModel) }, typeof(Cultures), Reuse.Singleton);
                c.RegisterMany(new[] { typeof(ILoadableViewModel), typeof(IUIColorsViewModel) }, typeof(UIColorsViewModel), Reuse.Singleton);

                //generic ViewModels
                c.Register<Scenes>(Reuse.Singleton);
                c.Register<AudioDevices>(Reuse.Singleton);
                c.Register<ShellViewModel>(Reuse.Singleton);
                c.Register<DialogViewModel>(Reuse.Singleton);
                c.Register<StatusbarViewModel>(Reuse.Singleton);
                c.Register<FileSystemViewModel>(Reuse.Singleton);
                c.Register<OptionsViewModel>(Reuse.Singleton);

                c.Register<CreateMediaItem>();
                c.Register<CreatePlaylist>();

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
                c.Register<IYoutubeUrlParseService, UrlParseService>();

                c.Register<IVersionService, VersionService>(Reuse.Singleton);
                c.Register<ILocalizationService, LocalizationService>(Reuse.Singleton);
                c.Register<ITranslationProvider, ResxTranslationProvider>(Reuse.Singleton);

                c.Register<IMessenger, MapleMessenger>(Reuse.Singleton);
                c.Register<IMapleMessageProxy, DefaultMessageProxy>(Reuse.Singleton);

                c.Register<ILoggingNotifcationService, LoggingNotifcationService>(Reuse.Singleton);
                c.Register<ILoggingService, LoggingService>(Reuse.Singleton);
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
