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
        private ITranslationManager _manager;

        protected override async void OnStartup(StartupEventArgs e)
        {
            InitializeLocalization();
            _container = await DependencyInjectionFactory.GetContainerAsync();
            _manager = _container.Resolve<ITranslationManager>();


            var colorsViewModel = _container.Resolve<UIColorsViewModel>();
            await colorsViewModel.LoadAsync();

            InitializeResources();

            base.OnStartup(e);

            var shell = new Shell(_manager, colorsViewModel)
            {
                DataContext = _container.Resolve<ShellViewModel>(),
            };

            shell.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await SaveState();
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

        private async Task SaveState()
        {
            var log = _container.Resolve<IMapleLog>();
            log.Info(Localization.Properties.Resources.SavingState);

            var tasks = new List<Task>
            {
                _manager.SaveAsync(),
            };

            _container.Resolve<IEnumerable<IRefreshable>>()
                      .ForEach(p => tasks.Add(p.SaveAsync()));


            await Task.WhenAll(tasks);

            log.Info(Localization.Properties.Resources.SavedState);
        }

        private void ExitInternal(ExitEventArgs e)
        {
            _container.Dispose();
            base.OnExit(e);
        }
    }
}
