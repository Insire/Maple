using DryIoc;
using Maple.Core;
using Maple.Properties;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Maple
{
    public partial class App : Application
    {
        private IContainer _container;
        private ITranslationService _manager;
        private IMapleLog _log;

        protected override async void OnStartup(StartupEventArgs e)
        {
            InitializeResources();
            InitializeLocalization();

            _container = DependencyInjectionFactory.Get();

            using (var vm = _container.Resolve<ISplashScreenViewModel>())
            {
                _manager = _container.Resolve<ITranslationService>();
                _log = _container.Resolve<IMapleLog>();

                var shell = new Shell(_manager, _container.Resolve<IUIColorsViewModel>(), _container.Resolve<ShellViewModel>());
                var screen = new SplashScreen(_manager, _container.Resolve<IUIColorsViewModel>(), vm);
                screen.Show();

                await Task.WhenAll(LoadApplicationData());

                shell.Loaded += (o, args) =>
                {
                    screen.Close();
                };

                _log.Info(Localization.Properties.Resources.AppStart);
                await Task.Delay(TimeSpan.FromSeconds(1));

                shell.Show();
            }

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            SaveState();
            ExitInternal(e);
        }

        private void InitializeResources()
        {
            var styles = CreateResourceDictionary(new Uri("/Maple;component/Resources/Style.xaml", UriKind.RelativeOrAbsolute));

            Resources.MergedDictionaries.Add(styles);
        }

        private IoCResourceDictionary CreateResourceDictionary(Uri uri)
        {
            // injecting the translation manager into a shared resourcedictionary,
            // so that hopefully all usages of the translation extension can be resolved inside of ResourceDictionaries

            var dic = new IoCResourceDictionary(_manager)
            {
                Source = uri,
            };
            dic.Add(typeof(ITranslationService).Name, _manager);

            return dic;
        }

        private void InitializeLocalization()
        {
            Thread.CurrentThread.CurrentCulture = Settings.Default.StartUpCulture;
        }

        private IList<Task> LoadApplicationData()
        {
            var tasks = new List<Task>();

            foreach (var item in _container.Resolve<IEnumerable<ILoadableViewModel>>())
                tasks.Add(item.LoadAsync());

            return tasks;
        }

        private void SaveState()
        {
            var log = _container.Resolve<IMapleLog>();
            log.Info(Localization.Properties.Resources.SavingState);

            foreach (var item in _container.Resolve<IEnumerable<ILoadableViewModel>>())
                item.Save();

            log.Info(Localization.Properties.Resources.SavedState);
        }

        private void ExitInternal(ExitEventArgs e)
        {
            _container.Dispose();
            base.OnExit(e);
        }
    }
}
