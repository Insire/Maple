using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using DryIoc;
using Maple.Data;
using Maple.Domain;
using Microsoft.Extensions.Logging;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public partial class App : Application
    {
        private IContainer _container;
        private Task _backgroundUpdate;

        protected override async void OnStartup(StartupEventArgs e)
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;

            _container = await DependencyInjectionFactory.Get().ConfigureAwait(true);

            var localizationService = _container.Resolve<ILocalizationService>();
            var loggerFactory = _container.Resolve<ILoggerFactory>();
            var log = _container.Resolve<ILoggingService>();

            InitializeUpdater(log);
            InitializeResources(localizationService);
            // TODO InitializeLocalization();

            using (var context = _container.Resolve<PlaylistContext>())
            {
                await context.Migrate().ConfigureAwait(false);
            }

            var shell = await GetShell(log).ConfigureAwait(true);
            shell.Show();

            base.OnStartup(e);
        }

        private async void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var log = _container?.Resolve<ILoggingService>();
            var dialog = _container?.Resolve<DialogViewModel>();

            log.Info(Localization.Properties.Resources.ExceptionMessageUnhandled);

            if (e.Exception.InnerException != null)
                log.Error(e.Exception.InnerException);

            log.Error(e.Exception);

            await dialog.ShowExceptionDialog(e.Exception).ConfigureAwait(true);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _backgroundUpdate.Wait();

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
        private async Task<Shell> GetShell(ILoggingService log)
        {
            using (var vm = _container.Resolve<ISplashScreenViewModel>())
            {
                var shell = _container.Resolve<Shell>();
                var screen = _container.Resolve<SplashScreen>();

                shell.Loaded += (o, args) => screen.Close();
                screen.Show();

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

        private void InitializeUpdater(ILoggingService log)
        {
            Splat.Locator.CurrentMutable.Register(() => log, typeof(Splat.ILogger));

            _backgroundUpdate = LoadUpdates(log);
        }

        private async Task LoadUpdates(ILoggingService log)
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
