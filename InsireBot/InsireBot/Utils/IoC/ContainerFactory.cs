using DryIoc;
using Maple.Core;
using Maple.Data;
using Maple.Youtube;
using System.Threading.Tasks;

namespace Maple
{
    public class ContainerFactory
    {
        public static Task<IContainer> InitializeIocContainer()
        {
            return Task.Run<IContainer>(() =>
           {
               var container = new Container(rules => rules.WithoutThrowOnRegisteringDisposableTransient());

               container.Register<IBotLog, LoggingService>(reuse: Reuse.Singleton);
               container.Register<IYoutubeUrlParseService, UrlParseService>();
               container.Register<Scenes>(reuse: Reuse.Singleton);
               container.Register<UIColorsViewModel>(reuse: Reuse.Singleton);

               container.Register<Playlists>(reuse: Reuse.Singleton);
               container.Register<AudioDevices>(reuse: Reuse.Singleton);
               container.Register<MediaPlayers>(reuse: Reuse.Singleton);
               container.Register<ShellViewModel>(reuse: Reuse.Singleton);
               container.Register<DialogViewModel>(reuse: Reuse.Singleton);
               container.Register<OptionsViewModel>(reuse: Reuse.Singleton);
               container.Register<DirectorViewModel>(reuse: Reuse.Singleton);
               container.Register<StatusbarViewModel>(reuse: Reuse.Singleton);

               container.Register<UrlParseService>();

               container.Register<ITranslationProvider, ResxTranslationProvider>(reuse: Reuse.Singleton);
               container.Register<ITranslationManager, TranslationManager>(reuse: Reuse.Singleton);
               container.Register<IMediaPlayer, NAudioMediaPlayer>(reuse: Reuse.Transient);

               container.Register<IMediaItemMapper, MediaItemMapper>();
               container.Register<IPlaylistMapper, PlaylistMapper>();

               container.Register<PlaylistContext>();

               return container;
           });
        }
    }
}
