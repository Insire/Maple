using InsireBot.Core;
using System.Reflection;

namespace InsireBot
{
    public class StatusbarViewModel : ObservableObject
    {
        private string _version;
        public string Version
        {
            get { return _version; }
            private set { SetValue(ref _version, value); }
        }

        public StatusbarViewModel()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            Version = $"v{version.Major}.{version.Minor}.{version.Revision}";
        }
    }
}
