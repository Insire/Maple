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
        private Task _backgroundUpdate;
        private ILogger _log;

        protected override async void OnStartup(StartupEventArgs e)
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;

            _container = await CompositionRoot.Get(e.Args).ConfigureAwait(true);

            var localizationService = _container.Resolve<ILocalizationService>();
            var loggerFactory = _container.Resolve<ILoggerFactory>();
            var weakEventManager = _container.Resolve<IScarletEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>>();

            _log = loggerFactory.CreateLogger<App>();

            InitializeUpdater(_log);

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
            _backgroundUpdate.Wait();

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

        private void InitializeUpdater(ILogger log)
        {
            Splat.Locator.CurrentMutable.Register(() => log, typeof(Splat.ILogger));

            _backgroundUpdate = LoadUpdates(log);
        }

        private async Task LoadUpdates(ILogger log)
        {
#if Release
            var manager = default(UpdateManager);
            try
            {
                using (manager = await UpdateManager.GitHubUpdateManager("https://www.github.com/Insire/Maple", prerelease: true).ConfigureAwait(true))
                {
                    await manager.UpdateApp().ConfigureAwait(true);

                    //Debug.WriteLine("CheckForUpdate");
                    //var updateInfo = await manager.CheckForUpdate(ignoreDeltaUpdates: true).ConfigureAwait(true);
                    //Debug.WriteLine("CheckForUpdate completed");
                    //if (updateInfo.ReleasesToApply.Any())
                    //{
                    //    Debug.WriteLine("Update found");
                    //    var releaseEntry = await manager.UpdateApp().ConfigureAwait(true);
                    //    Debug.WriteLine($"Update complete {releaseEntry.Version}");

                    //}
                }
            }
            catch (WebException ex)
            {
                Debug.WriteLine(ex.Status);
                log.Error(ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                log.Error(ex);
            }
            finally
            {
                manager?.Dispose();
            }
#endif
        }

        private void ExitInternal(ExitEventArgs e)
        {
            DispatcherUnhandledException -= App_DispatcherUnhandledException;

            _container.Dispose();
            base.OnExit(e);
        }
    }
}
