using DryIoc;
using log4net;
using System.Reflection;
using System.Windows;

namespace InsireBot
{
    public partial class App : Application
    {
        internal static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private IContainer _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();

            base.OnStartup(e);

            InitIoC();

            //var locator = _container.Resolve<GlobalServiceLocator>();

            var shell = new Shell(_container)
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

        private void InitIoC()
        {
            _container = new Container(rules: Rules.Default, scopeContext: new AsyncExecutionFlowScopeContext());
            _container.Register<GlobalServiceLocator>(reuse: Reuse.Singleton);
            _container.Register<Scenes>(reuse: Reuse.Singleton);
            _container.Register<UIColorsViewModel>(reuse: Reuse.Singleton);

            //if (ViewModelBase.IsInDesignModeStatic)
            //{
            //    // Create design time view services and models
            //    _container.Register<IDataService, DesignTimeDataService>();
            //}
            //else
            //{
            // Create run time view services and models
            _container.Register<IDataService, RuntimeDataService>();
            //}

            _container.Register<DataParsingService>();
            _container.Register<MediaItemsStore>();
            _container.Register<PlaylistsViewModel>(reuse: Reuse.Singleton);
            _container.Register<MediaPlayerViewModel>();
            _container.Register<CreateMediaItemViewModel>();
            _container.Register<CreatePlaylistViewModel>();
            _container.Register<StatusbarViewModel>(reuse: Reuse.Singleton);
            _container.Register<ShellViewModel>(reuse: Reuse.Singleton);
            _container.Register<OptionsViewModel>(reuse: Reuse.Singleton);
            //_container.Register<IMediaItem, MediaItem>();
            _container.Register<ITranslationProvider, ResxTranslationProvider>(reuse: Reuse.Singleton);
            _container.Register<ITranslationManager, TranslationManager>(reuse: Reuse.Singleton);
        }
    }
}
