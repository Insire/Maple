using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using DryIoc;
using Microsoft.Extensions.Logging;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public partial class App : Application
    {
        private DryIoc.IContainer _container;
        private ILogger _log;

        protected override async void OnStartup(StartupEventArgs e)
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;

            _container = await CompositionRoot.Get(e.Args).ConfigureAwait(true);

            var localizationService = _container.Resolve<ILocalizationService>();
            var loggerFactory = _container.Resolve<ILoggerFactory>();
            var weakEventManager = _container.Resolve<IScarletEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>>();

            _log = loggerFactory.CreateLogger<App>();

            Resources.MergedDictionaries.Add(new IoCResourceDictionary(localizationService, weakEventManager, new Uri("/Maple;component/Resources/Style.xaml", UriKind.RelativeOrAbsolute)));

            var shell = await GetShell(_log).ConfigureAwait(true);
            shell.DataContext = _container.Resolve<ShellViewModel>();
            shell.Show();

            base.OnStartup(e);
        }

        private async void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            _log.LogInformation(Maple.Properties.Resources.ExceptionMessageUnhandled);
            _log.LogError(e.Exception, e.Exception.Message);

            var dialog = _container.Resolve<DialogViewModel>();
            await dialog.ShowExceptionDialog(e.Exception).ConfigureAwait(true);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ExitInternal(e);
        }

        /// <remarks>
        /// order matters alot here, so be careful when modifying this
        /// </remarks>
        private async Task<Shell> GetShell(ILogger log)
        {
            using (var vm = _container.Resolve<SplashScreenViewModel>())
            {
                var shell = _container.Resolve<Shell>();
                var screen = _container.Resolve<SplashScreen>();

                shell.Loaded += (o, args) => screen.Close();
                screen.Show();

                log.LogInformation(Maple.Properties.Resources.AppStart);
                await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(true);

                return shell;
            }
        }

        private void ExitInternal(ExitEventArgs e)
        {
            DispatcherUnhandledException -= App_DispatcherUnhandledException;

            _container.Dispose();
            base.OnExit(e);
        }
    }
}
