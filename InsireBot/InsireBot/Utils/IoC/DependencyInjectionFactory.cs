using DryIoc;
using Maple.Core;
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
        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <returns></returns>
        public static IContainer GetContainer()
        {
            var container = new Container(rules => rules.WithoutThrowOnRegisteringDisposableTransient());

            RegisterViewModels();
            RegisterServices();

            container.Resolve<ITranslationService>().Load();

            if (Debugger.IsAttached)
                Debugging();

            return container;

            void RegisterViewModels()
            {
                // save-/loadable ViewModels
                container.Register<Playlists>(Reuse.Singleton);
                container.Register<MediaPlayers>(Reuse.Singleton);
                container.Register<OptionsViewModel>(Reuse.Singleton);
                container.Register<UIColorsViewModel>(Reuse.Singleton);

                container.RegisterMany(
                    new[]
                    {
                        typeof(Playlists),
                        typeof(MediaPlayers),
                        typeof(OptionsViewModel),
                        typeof(UIColorsViewModel)
                    },
                    serviceTypeCondition: type => type.IsInterface);

                //generic ViewModels
                container.Register<Scenes>(Reuse.Singleton);
                container.Register<AudioDevices>(Reuse.Singleton);
                container.Register<ShellViewModel>(Reuse.Singleton);
                container.Register<DialogViewModel>(Reuse.Singleton);
                container.Register<StatusbarViewModel>(Reuse.Singleton);
                container.Register<FileSystemViewModel>(Reuse.Singleton);
                container.Register<SplashScreenViewModel>(Reuse.Singleton);

                container.Register<ILoadableViewModel, RefreshableDecorator>(setup: Setup.Decorator);
            };

            void RegisterServices()
            {
                container.Register<IMediaPlayer, NAudioMediaPlayer>(Reuse.Transient);
                container.Register<IMediaRepository, MediaRepository>();
                container.Register<IMediaItemMapper, MediaItemMapper>();
                container.Register<IPlaylistMapper, PlaylistMapper>();
                container.Register<IMapleLog, LoggingService>(Reuse.Singleton);
                container.Register<IYoutubeUrlParseService, UrlParseService>();
                container.Register<ITranslationProvider, ResxTranslationProvider>(Reuse.Singleton);
                container.Register<ITranslationService, TranslationService>(Reuse.Singleton);
                container.Register<ISequenceProvider, SequenceService>();
            }

            void Debugging()
            {
                foreach (var item in container.VerifyResolutions())
                {
                    Debug.WriteLine($"{item.Key} registered with {item.Value}");
                }

                foreach (var item in container.GetServiceRegistrations().OrderBy(p => p.Factory.ImplementationType.ToString()))
                {
                    Debug.WriteLine($"{item.ServiceType.Name.PadRight(30, '.')} registered with {item.Factory.FactoryID.ToString().PadRight(10, '.')} of type {item.Factory.ImplementationType.Name}");
                }
            }
        }
    }
}
