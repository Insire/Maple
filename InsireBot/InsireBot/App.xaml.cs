using System.Windows;
using log4net;

namespace InsireBotWPF
{
    public partial class App : Application
    {
        internal static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnStartup(StartupEventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            Log.Info("InsireBot started");
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            GlobalServiceLocator.Cleanup();
            Log.Info("InsireBot closed");
            base.OnExit(e);
        }
    }
}
