using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using DryIoc;
using Jot;
using Jot.Storage;
using Microsoft.Extensions.Logging;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public partial class App : Application
    {
        private DryIoc.IContainer _container;
        private ILogger _log;

        private readonly Tracker _tracker;

        public App()
        {
            _tracker = new Tracker(new JsonFileStore(Environment.SpecialFolder.ApplicationData));

            _tracker.Configure<Shell>()
                .Id(w => w.Name)
                .Properties(w => new { w.Height, w.Width, w.Left, w.Top, w.WindowState })
                .PersistOn(nameof(Window.Closing))
                .StopTrackingOn(nameof(Window.Closing));
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DispatcherUnhandledException += App_DispatcherUnhandledException;

            _container = CompositionRoot.Get();
            _container.UseInstance(_tracker);

            var localizationService = _container.Resolve<ILocalizationService>();
            var loggerFactory = _container.Resolve<ILoggerFactory>();
            var weakEventManager = _container.Resolve<IScarletEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>>();

            _log = loggerFactory.CreateLogger<App>();

            Resources.MergedDictionaries.Add(new IoCResourceDictionary(localizationService, weakEventManager, new Uri("/Maple;component/Resources/Style.xaml", UriKind.RelativeOrAbsolute)));

            var shell = await GetShell(_log);

            shell.Show();
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
                var shellViewModel = _container.Resolve<ShellViewModel>();
                var loading = shellViewModel.Load(CancellationToken.None);

                shell.DataContext = shellViewModel;
                shell.Loaded += async (o, args) =>
                {
                    await loading;
                    screen.Close();
                };

                screen.Show();

                log.LogInformation(Maple.Properties.Resources.AppStart);
                await Task.WhenAll(loading, Task.Delay(TimeSpan.FromSeconds(1)));

                return shell;
            }
        }

        private void ExitInternal(ExitEventArgs e)
        {
            DispatcherUnhandledException -= App_DispatcherUnhandledException;

            var tracker = _container.Resolve<Tracker>();
            tracker.PersistAll();

            _container.Dispose();
            base.OnExit(e);
        }
    }
}
