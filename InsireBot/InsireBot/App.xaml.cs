using DryIoc;
using InsireBot.Core;
using InsireBot.Data;
using InsireBot.Properties;
using InsireBot.Youtube;
using System.IO;
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

            var manager = _container.Resolve<ITranslationManager>();
            var colorsViewModel = _container.Resolve<UIColorsViewModel>();
            var shell = new Shell(manager, colorsViewModel)
            {
                DataContext = _container.Resolve<ShellViewModel>(),
            };

            colorsViewModel.ApplyColorsFromSettings();
            shell.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            SaveState();
            ExitInternal(e);
        }

        private void InitializeLocalization()
        {
            Thread.CurrentThread.CurrentCulture = Settings.Default.StartUpCulture;
        }

        private void InitializeIocContainer()
        {
            var connection = new DBConnection(new DirectoryInfo(".").FullName);
            _container = new Container(rules => rules.WithoutThrowOnRegisteringDisposableTransient());

            _container.RegisterInstance(connection);

            _container.Register<IBotLog, LoggingService>(reuse: Reuse.Singleton);
            _container.Register<IYoutubeUrlParseService, UrlParseService>();
            _container.Register<Scenes>(reuse: Reuse.Singleton);
            _container.Register<UIColorsViewModel>(reuse: Reuse.Singleton);
            _container.Register<UrlParseService>();
            _container.Register<MediaPlayers>(reuse: Reuse.Singleton);
            _container.Register<Playlists>(reuse: Reuse.Singleton);
            _container.Register<DirectorViewModel>(reuse: Reuse.Singleton);
            _container.Register<CreateMediaItemViewModel>();
            _container.Register<CreatePlaylistViewModel>();
            _container.Register<StatusbarViewModel>(reuse: Reuse.Singleton);
            _container.Register<ShellViewModel>(reuse: Reuse.Singleton);
            _container.Register<OptionsViewModel>(reuse: Reuse.Singleton);

            _container.Register<ITranslationProvider, ResxTranslationProvider>(reuse: Reuse.Singleton);
            _container.Register<ITranslationManager, TranslationManager>(reuse: Reuse.Singleton);
            _container.Register<IMediaPlayer, NAudioMediaPlayer>(reuse: Reuse.Transient);

            _container.Register<IMediaItemMapper, MediaItemMapper>();
            _container.Register<IPlaylistMapper, PlaylistMapper>();

            _container.Register<IPlaylistsRepository, PlaylistsRepository>(reuse: Reuse.Singleton);
            _container.Register<IMediaItemRepository, MediaItemRepository>(reuse: Reuse.Singleton);
            _container.Register<IMediaPlayerRepository, MediaPlayerRepository>(reuse: Reuse.Singleton);
        }

        private void SaveState()
        {
            var log = _container.Resolve<IBotLog>();
            log.Info(Localization.Properties.Resources.SavingState);

            _container.Resolve<Playlists>().Save();
            _container.Resolve<MediaPlayers>().Save();
            _container.Resolve<ITranslationManager>().Save();

            log.Info(Localization.Properties.Resources.SavedState);
        }

        private void ExitInternal(ExitEventArgs e)
        {
            _container.Dispose();
            base.OnExit(e);
        }
    }
}
