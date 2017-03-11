using DryIoc;
using Maple.Core;
using Maple.Youtube;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maple
{
    public class DependencyInjectionFactory
    {
        public static Task<IContainer> GetContainerAsync()
        {
            return Task.Run<IContainer>(async () =>
           {
               var container = new Container(rules => rules.WithoutThrowOnRegisteringDisposableTransient());

               RegisterViewModels();
               RegisterServices();

               container.Register<Scenes>(Reuse.Singleton);
               await container.Resolve<ITranslationManager>().LoadAsync();

               var tasks = new List<Task>();
               foreach (var item in container.Resolve<IEnumerable<IRefreshable>>())
                   tasks.Add(item.LoadAsync());

               await Task.WhenAll(tasks);

               return container;

               void RegisterViewModels()
               {
                   // save-/loadable ViewModels
                   container.Register<Playlists>(Reuse.Singleton);
                   container.Register<MediaPlayers>(Reuse.Singleton);
                   container.Register<OptionsViewModel>(Reuse.Singleton);
                   container.Register<UIColorsViewModel>(Reuse.Singleton);

                   // generic ViewModels
                   container.Register<AudioDevices>(Reuse.Singleton);
                   container.Register<ShellViewModel>(Reuse.Singleton);
                   container.Register<DialogViewModel>(Reuse.Singleton);
                   container.Register<DirectorViewModel>(Reuse.Singleton);
                   container.Register<StatusbarViewModel>(Reuse.Singleton);

                   // "Overloads" for already registered ViewModels, so i can query them via the container by resolving the specified interface
                   container.Register<IRefreshable, Playlists>(ifAlreadyRegistered: IfAlreadyRegistered.AppendNewImplementation);
                   container.Register<IRefreshable, MediaPlayers>(ifAlreadyRegistered: IfAlreadyRegistered.AppendNewImplementation);
                   container.Register<IRefreshable, OptionsViewModel>(ifAlreadyRegistered: IfAlreadyRegistered.AppendNewImplementation);
                   container.Register<IRefreshable, UIColorsViewModel>(ifAlreadyRegistered: IfAlreadyRegistered.AppendNewImplementation);

                   // Decorator for logging Loading and Saving
                   container.Register<IRefreshable, RefreshableDecorator>(setup: Setup.Decorator);
               };

               void RegisterServices()
               {
                   // misc
                   container.Register<IMediaPlayer, NAudioMediaPlayer>(Reuse.Transient);

                   container.Register<IMediaItemMapper, MediaItemMapper>();
                   container.Register<IPlaylistMapper, PlaylistMapper>();
                   container.Register<IMapleLog, LoggingService>(Reuse.Singleton);
                   container.Register<IYoutubeUrlParseService, UrlParseService>();
                   container.Register<ITranslationProvider, ResxTranslationProvider>(Reuse.Singleton);
                   container.Register<ITranslationManager, TranslationManager>(Reuse.Singleton);
               }
           });
        }
    }
}
