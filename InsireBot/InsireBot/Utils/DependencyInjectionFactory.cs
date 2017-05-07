using DryIoc;
using Maple.Core;
using Maple.Data;
using Maple.Youtube;
using System.Diagnostics;
using System.Linq;

namespace Maple
{
    /// <summary>
    /// Factory class that provides an Instance of <see cref="IContainer"/>
    /// </summary>
    public class DependencyInjectionFactory
    {
        public static IContainer Get()
        {
            var c = new Container();

            RegisterViewModels();
            RegisterServices();
            RegisterControls();

            c.Resolve<ITranslationService>().Load();

            if (Debugger.IsAttached)
                Debugging();

            return c;

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
                c.RegisterMany(new[] { typeof(ILoadableViewModel), typeof(ICultureViewModel) }, typeof(CultureViewModel), Reuse.Singleton);
                c.RegisterMany(new[] { typeof(ILoadableViewModel), typeof(IUIColorsViewModel) }, typeof(UIColorsViewModel), Reuse.Singleton);

                //generic ViewModels
                c.Register<Scenes>(Reuse.Singleton);
                c.Register<AudioDevices>(Reuse.Singleton);
                c.Register<ShellViewModel>(Reuse.Singleton);
                c.Register<DialogViewModel>(Reuse.Singleton);
                c.Register<StatusbarViewModel>(Reuse.Singleton);
                c.Register<FileSystemViewModel>(Reuse.Singleton);
                c.Register<OptionsViewModel>(Reuse.Singleton);
                c.Register<ISplashScreenViewModel, SplashScreenViewModel>(Reuse.Singleton);
            };

            void RegisterServices()
            {
                c.Register<IMediaPlayer, NAudioMediaPlayer>(setup: Setup.With(allowDisposableTransient: true));
                c.Register<IPlaylistContext, PlaylistContext>(setup: Setup.With(allowDisposableTransient: true));
                c.Register<IMediaRepository, MediaRepository>(setup: Setup.With(allowDisposableTransient: true));

                c.Register<IPlaylistMapper, PlaylistMapper>();
                c.Register<IMediaPlayerMapper, MediaPlayerMapper>();
                c.Register<IMediaItemMapper, MediaItemMapper>();
                c.Register<ISequenceProvider, SequenceService>();
                c.Register<IYoutubeUrlParseService, UrlParseService>();

                c.Register<IMapleLog, LoggingService>(Reuse.Singleton);
                c.Register<IVersionService, VersionService>(Reuse.Singleton);
                c.Register<ITranslationService, TranslationService>(Reuse.Singleton);
                c.Register<ITranslationProvider, ResxTranslationProvider>(Reuse.Singleton);
            }

            void Debugging()
            {
                foreach (var item in c.VerifyResolutions())
                {
                    Debug.WriteLine($"{item.Key} registered with {item.Value}");
                }

                foreach (var item in c.GetServiceRegistrations().OrderBy(p => p.Factory.ImplementationType.ToString()).ToList())
                {
                    Debug.WriteLine($"{item.ServiceType.Name.PadRight(30, '.')} registered with {item.Factory.FactoryID.ToString().PadRight(10, '.')} of type {item.Factory.ImplementationType.Name}");
                }
            }

        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <returns></returns>
        public static IContainer GetTestContainer()
        {
            var c = new Container();

            RegisterViewModels();
            RegisterServices();

            c.Resolve<ITranslationService>().Load();

            if (Debugger.IsAttached)
                Debugging();

            return c;

            void RegisterViewModels()
            {
                // save-/loadable ViewModels
                c.Register<IPlaylistsViewModel, Playlists>(Reuse.Singleton);
                c.Register<IMediaPlayersViewModel, MediaPlayers>(Reuse.Singleton, setup: Setup.With(allowDisposableTransient: true));
                c.Register<ICultureViewModel, CultureViewModel>(Reuse.Singleton);
                c.Register<IUIColorsViewModel, UIColorsViewModel>(Reuse.Singleton);

                c.RegisterMany(
                    new[]
                    {
                        typeof(Playlists),
                        typeof(MediaPlayers),
                        typeof(CultureViewModel),
                        typeof(UIColorsViewModel)
                    },
                    serviceTypeCondition: type => type.IsInterface, setup: Setup.With(allowDisposableTransient: true));

                //generic ViewModels
                c.Register<Scenes>(Reuse.Singleton);
                c.Register<AudioDevices>(Reuse.Singleton);
                c.Register<ShellViewModel>(Reuse.Singleton);
                c.Register<DialogViewModel>(Reuse.Singleton);
                c.Register<StatusbarViewModel>(Reuse.Singleton);
                c.Register<FileSystemViewModel>(Reuse.Singleton);
                c.Register<ISplashScreenViewModel, SplashScreenViewModel>(Reuse.Singleton);

                //c.RegisterMapping<ILoadableViewModel, IPlaylistsViewModel>();
                //c.RegisterMapping<ILoadableViewModel, IMediaPlayersViewModel>();
                //c.RegisterMapping<ILoadableViewModel, IOptionsViewModel>();
                //c.RegisterMapping<ILoadableViewModel, IUIColorsViewModel>();
            };

            void RegisterServices()
            {
                c.Register<IMediaPlayer, NAudioMediaPlayer>(Reuse.Transient, setup: Setup.With(allowDisposableTransient: true));
                c.Register<IMediaRepository, MediaRepository>(setup: Setup.With(allowDisposableTransient: true));
                c.Register<IMediaItemMapper, MediaItemMapper>();
                c.Register<IPlaylistMapper, PlaylistMapper>();
                c.Register<IMapleLog, LoggingService>(Reuse.Singleton);
                c.Register<IYoutubeUrlParseService, UrlParseService>();
                c.Register<ITranslationProvider, ResxTranslationProvider>(Reuse.Singleton);
                c.Register<ITranslationService, TranslationService>(Reuse.Singleton);
                c.Register<ISequenceProvider, SequenceService>();
                c.Register<IVersionService, VersionService>(Reuse.Singleton);
            }

            void Debugging()
            {
                foreach (var item in c.VerifyResolutions())
                {
                    Debug.WriteLine($"{item.Key} registered with {item.Value}");
                }

                foreach (var item in c.GetServiceRegistrations().OrderBy(p => p.Factory.ImplementationType.ToString()).ToList())
                {
                    Debug.WriteLine($"{item.ServiceType.Name.PadRight(30, '.')} registered with {item.Factory.FactoryID.ToString().PadRight(10, '.')} of type {item.Factory.ImplementationType.Name}");
                }
            }
        }
    }
}
