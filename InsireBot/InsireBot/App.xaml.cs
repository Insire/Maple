using DryIoc;
using Maple.Core;
using Maple.Properties;
using System;
using System.Collections.Generic;
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
            InitializeResources();
            InitializeLocalization();

            _container = DependencyInjectionFactory.GetContainer();
            _manager = _container.Resolve<ITranslationManager>();

            var shell = new Shell(_manager, _container.Resolve<UIColorsViewModel>())
            {
                DataContext = _container.Resolve<ShellViewModel>(),
            };

            shell.Show();

            foreach (var item in _container.Resolve<IEnumerable<ILoadableViewModel>>())
                item.Load();

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
            dic.Add(typeof(ITranslationManager).Name, _manager);

            return dic;
        }

        private void InitializeLocalization()
        {
            Thread.CurrentThread.CurrentCulture = Settings.Default.StartUpCulture;
        }

        private void SaveState()
        {
            var log = _container.Resolve<IMapleLog>();
            log.Info(Localization.Properties.Resources.SavingState);

            _container.Resolve<IEnumerable<ILoadableViewModel>>()
                      .ForEach(p => p.Save());

            log.Info(Localization.Properties.Resources.SavedState);
        }

        private void ExitInternal(ExitEventArgs e)
        {
            _container.Dispose();
            base.OnExit(e);
        }
    }
}
