using InsireBot.Core;
using System.Reflection;

namespace InsireBot
{
    public class StatusbarViewModel : ObservableObject
    {
        private readonly ITranslationManager _manager;

        private string _version;
        public string Version
        {
            get { return _version; }
            private set { SetValue(ref _version, value); }
        }

        private string _language;
        public string Language
        {
            get { return _language; }
            private set { SetValue(ref _language, value); }
        }

        public StatusbarViewModel(ITranslationManager manager)
        {
            _manager = manager;
            _manager.PropertyChanged += (o, e) =>
              {
                  if (e.PropertyName == $"{nameof(ITranslationManager.CurrentLanguage)}")
                      UpdateLanguage();
              };

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            Version = $"v{version.Major}.{version.Minor}.{version.Revision}";

            UpdateLanguage();
        }

        private void UpdateLanguage()
        {
            Language = $"({_manager.CurrentLanguage.TwoLetterISOLanguageName})";
        }

        // TODO notifications?
    }
}
