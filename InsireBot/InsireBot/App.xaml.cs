using DryIoc;
using Maple.Core;
using Maple.Data;
using Maple.Properties;
using Maple.Youtube;
using System;
using System.IO;
using System.Threading;
using System.Windows;

namespace Maple
{
    public partial class App : Application
    {
        private IContainer _container;
        private ITranslationManager _manager;

        protected override void OnStartup(StartupEventArgs e)
        {
            InitializeLocalization();
            InitializeIocContainer();
            InitializeResources();

            base.OnStartup(e);

            var colorsViewModel = _container.Resolve<UIColorsViewModel>();
            var shell = new Shell(_manager, colorsViewModel)
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

        private void InitializeResources()
        {
            var styles = CreateResourceDictionary(new Uri("/Maple;component/Resources/Style.xaml", UriKind.RelativeOrAbsolute));
            var listViewStyles = CreateResourceDictionary(new Uri("/Maple;component/Resources/ListViewStyles.xaml", UriKind.RelativeOrAbsolute));

            Resources.MergedDictionaries.Add(styles);
            Resources.MergedDictionaries.Add(listViewStyles);
        }

        private IoCResourceDictionary CreateResourceDictionary(Uri uri)
        {
            // injecting the translation manager into a shared resourcedictionary,
            // so that hopefully all usages of the translation extension can be resolved inside of ResourceDictionaries

            var dic = new IoCResourceDictionary(_manager)
            {
                Source = uri,
            };
            dic.Add(typeof(ITranslationManager).Name, _manager);

            return dic;
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

            _container.Register<Playlists>(reuse: Reuse.Singleton);
            _container.Register<MediaPlayers>(reuse: Reuse.Singleton);
            _container.Register<ShellViewModel>(reuse: Reuse.Singleton);
            _container.Register<DialogViewModel>(reuse: Reuse.Singleton);
            _container.Register<OptionsViewModel>(reuse: Reuse.Singleton);
            _container.Register<DirectorViewModel>(reuse: Reuse.Singleton);
            _container.Register<StatusbarViewModel>(reuse: Reuse.Singleton);

            _container.Register<UrlParseService>();

            _container.Register<ITranslationProvider, ResxTranslationProvider>(reuse: Reuse.Singleton);
            _container.Register<ITranslationManager, TranslationManager>(reuse: Reuse.Singleton);
            _container.Register<IMediaPlayer, NAudioMediaPlayer>(reuse: Reuse.Transient);

            _container.Register<IMediaItemMapper, MediaItemMapper>();
            _container.Register<IPlaylistMapper, PlaylistMapper>();

            _container.Register<IPlaylistContext, PlaylistContext>();

            _manager = _container.Resolve<ITranslationManager>();
        }

        private void SaveState()
        {
            var log = _container.Resolve<IBotLog>();
            log.Info(Localization.Properties.Resources.SavingState);

            _container.Resolve<Playlists>().Save();
            _container.Resolve<MediaPlayers>().Save();
            _manager.Save();

            log.Info(Localization.Properties.Resources.SavedState);
        }

        private void ExitInternal(ExitEventArgs e)
        {
            _container.Dispose();
            base.OnExit(e);
        }
    }
}
