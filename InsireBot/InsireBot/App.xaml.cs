using DryIoc;
using InsireBot.Core;
using InsireBot.Data;
using InsireBot.Properties;
using InsireBot.Youtube;
using System;
using System.Threading;
using System.Windows;

namespace InsireBot
{
    public partial class App : Application
    {
        private IContainer _container;

        protected override void OnStartup(StartupEventArgs e)
        {
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
            _container = new Container(rules => rules.WithoutThrowOnRegisteringDisposableTransient());

            _container.Register<IBotLog, LoggingService>();
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
            _container.Register<IMediaPlayer, NAudioMediaPlayer>(reuse: Reuse.Transient);
            _container.Register<IPlaylistsRepository, PlaylistsRepository>();
            _container.Register<IMediaItemRepository, MediaItemRepository>();
            _container.Register<IMediaItemMapper, MediaItemMapper>();
            _container.Register<IYoutubeUrlParseService, UrlParseService>();
            _container.Register<IPlaylistMapper, PlaylistMapper>();

        }

        private Type GetTypeInternal()
        {
            return typeof(App);
        }
    }
}
