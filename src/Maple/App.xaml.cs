using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DryIoc;
using Maple.Core;
using Maple.Domain;
using Squirrel;

namespace Maple
{
    public partial class App : Application
    {
        private IContainer _container;

        protected override async void OnStartup(StartupEventArgs e)
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;

            _container = await DependencyInjectionFactory.Get().ConfigureAwait(true);

            var localizationService = _container.Resolve<ILocalizationService>();
            var log = _container.Resolve<ILoggingService>();

            InitializeResources(localizationService);
            InitializeLocalization();

            var shell = await GetShell(localizationService, log).ConfigureAwait(true);
            shell.Show();

            base.OnStartup(e);
        }

        private async void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var log = _container?.Resolve<ILoggingService>();
            var dialog = _container?.Resolve<IDialogViewModel>();

            log.Info(Localization.Properties.Resources.ExceptionMessageUnhandled);

            if (e.Exception.InnerException != null)
                log.Error(e.Exception.InnerException);

            log.Error(e.Exception);

            await dialog.ShowExceptionDialog(e.Exception).ConfigureAwait(true);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            SaveState();
            DisposeResources();
            ExitInternal(e);
        }

        /// <summary>
        /// Gets the shell control.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        /// <remarks>
        /// order matters alot here, so be careful when modifying this
        /// </remarks>
        private async Task<Shell> GetShell(ILocalizationService service, ILoggingService log)
        {
            using (var vm = _container.Resolve<ISplashScreenViewModel>())
            {
                var shell = _container.Resolve<Shell>();
                var screen = _container.Resolve<SplashScreen>();

                shell.Loaded += (o, args) => screen.Close();
                screen.Show();

                await Task.WhenAll(LoadApplicationData()).ConfigureAwait(true);

                log.Info(Localization.Properties.Resources.AppStart);
                await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(true);

                return shell;
            }
        }

        /// <summary>
        /// Initializes the resources.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <remarks>
        /// injecting the translation manager into a SharedResourcedictionary,
        /// so that hopefully all usages of the translation extension can be resolved inside of ResourceDictionaries
        /// </remarks>
        private void InitializeResources(ILocalizationService service)
        {
            var url = new Uri("/Maple;component/Resources/Style.xaml", UriKind.RelativeOrAbsolute);
            var styles = new IoCResourceDictionary(service, url);

            Resources.MergedDictionaries.Add(styles);
        }

        private void InitializeLocalization()
        {
            Thread.CurrentThread.CurrentCulture = Core.Properties.Settings.Default.StartUpCulture;
        }

        private async Task LoadUpdates()
        {
            using (var manager = new UpdateManager("https://github.com/Insire/Maple/releases/latest"))
                await manager.UpdateApp().ConfigureAwait(true);
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
            var log = _container.Resolve<ILoggingService>();
            log.Info(Localization.Properties.Resources.SavingState);

            foreach (var item in _container.Resolve<IEnumerable<ILoadableViewModel>>())
                item.Save();

            log.Info(Localization.Properties.Resources.SavedState);
        }

        private void DisposeResources()
        {
            // TODO get and dispose claimed resources
        }

        private void ExitInternal(ExitEventArgs e)
        {
            DispatcherUnhandledException -= App_DispatcherUnhandledException;

            _container.Dispose();
            base.OnExit(e);
        }
    }
}
