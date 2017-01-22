using DryIoc;
using InsireBot.Core;
using InsireBot.Data;
using InsireBot.Properties;
using InsireBot.Youtube;
using log4net;
using System.Threading;
using System.Windows;

namespace InsireBot
{
    public partial class App : Application
    {
        private IContainer _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();

            base.OnStartup(e);

            InitializeLocalization();
            InitializeIocContainer();
            InitializeTheme();

            var manager = _container.Resolve<ITranslationManager>();
            var shell = new Shell(manager)
            {
                DataContext = _container.Resolve<ShellViewModel>(),
            };

            shell.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _container.Dispose();
            base.OnExit(e);
        }

        private void InitializeTheme()
        {
            UIColorsViewModel.ApplyColorsFromSettings();
        }

        private void InitializeLocalization()
        {
            Thread.CurrentThread.CurrentCulture = Settings.Default.StartUpCulture;
        }

        private void InitializeIocContainer()
        {
            _container = new Container(rules: Rules.Default, scopeContext: new AsyncExecutionFlowScopeContext());

            _container.Register<IBotLog>(made: Made.Of(() => LogManager.GetLogger(typeof(App))));
            _container.Register<GlobalServiceLocator>(reuse: Reuse.Singleton);
            _container.Register<Scenes>(reuse: Reuse.Singleton);
            _container.Register<UIColorsViewModel>(reuse: Reuse.Singleton);
            _container.Register<UrlParseService>();
            _container.Register<PlaylistViewModel>(reuse: Reuse.Singleton);
            _container.Register<MediaPlayerViewModel>();
            _container.Register<CreateMediaItemViewModel>();
            _container.Register<CreatePlaylistViewModel>();
            _container.Register<StatusbarViewModel>(reuse: Reuse.Singleton);
            _container.Register<ShellViewModel>(reuse: Reuse.Singleton);
            _container.Register<OptionsViewModel>(reuse: Reuse.Singleton);

            _container.Register<ITranslationProvider, ResxTranslationProvider>(reuse: Reuse.Singleton);
            _container.Register<ITranslationManager, TranslationManager>(reuse: Reuse.Singleton);
            _container.Register<IMediaPlayer, NAudioMediaPlayer>();
            _container.Register<IPlaylistsRepository, PlaylistsRepository>();
            _container.Register<IMediaItemRepository, MediaItemRepository>();
            _container.Register<IMediaItemMapper, MediaItemMapper>();
            _container.Register<IYoutubeUrlParseService, UrlParseService>();
            _container.Register<IPlaylistMapper, PlaylistMapper>();
            _container.Register<IMediaItemMapper, MediaItemMapper>();
        }
    }
}
