using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using DryIoc;
using Jot;
using Microsoft.Extensions.Logging;

namespace Maple
{
    public partial class App : Application
    {
        private readonly Tracker _tracker;

        private readonly IContainer _container;
        private readonly ILogger _log;

        public App()
        {
            _container = CompositionRoot.Get();
            _tracker = _container.Resolve<Tracker>();
            _log = _container.Resolve<ILoggerFactory>().CreateLogger<App>();

            _log.LogInformation(Maple.Properties.Resources.AppStart);
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            _log.LogInformation(Maple.Properties.Resources.AppStartLoadUI);
            base.OnStartup(e);

            DispatcherUnhandledException += App_DispatcherUnhandledException;

            var styles = _container.Resolve<IoCResourceDictionary>();

            Resources.MergedDictionaries.Add(styles);

            _log.LogInformation(Maple.Properties.Resources.AppStartLoadData);
            var shell = await GetShell();

            shell.Show();
            _log.LogInformation(Maple.Properties.Resources.AppStartComplete);
        }

        private async void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            _log.LogInformation(Maple.Properties.Resources.ExceptionMessageUnhandled);
            _log.LogError(e.Exception, e.Exception.Message);

            var dialog = _container.Resolve<DialogViewModel>();
            await dialog.ShowExceptionDialog(e.Exception);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _log.LogInformation(Maple.Properties.Resources.AppExit);
            ExitInternal(e);
        }

        /// <remarks>
        /// order matters alot here, so be careful when modifying this
        /// </remarks>
        private async Task<Shell> GetShell()
        {
            using (var vm = _container.Resolve<SplashScreenViewModel>())
            {
                var shell = _container.Resolve<Shell>();
                var screen = _container.Resolve<SplashScreen>();
                var shellViewModel = _container.Resolve<ShellViewModel>();
                var loading = shellViewModel.Load(CancellationToken.None);

                shell.DataContext = shellViewModel;
                shell.Loaded += async (o, args) =>
                {
                    await loading;
                    screen.Close();
                };

                screen.Show();

                await Task.WhenAll(loading, Task.Delay(TimeSpan.FromSeconds(1)));

                return shell;
            }
        }

        private void ExitInternal(ExitEventArgs e)
        {
            DispatcherUnhandledException -= App_DispatcherUnhandledException;

            _log.LogInformation(Maple.Properties.Resources.AppExitSaving);
            _tracker.PersistAll();

            _log.LogInformation(Maple.Properties.Resources.AppExitComplete);
            _container.Dispose();

            base.OnExit(e);
        }
    }
}
