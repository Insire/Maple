using Maple.Core;
using System.Linq;
using System.Reflection;

namespace Maple
{
    public class StatusbarViewModel : ObservableObject
    {
        private readonly MediaPlayers _mediaplayers;
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

        private MainMediaPlayer _mainMediaPlayer;
        public MainMediaPlayer MainMediaPlayer
        {
            get { return _mainMediaPlayer; }
            private set { SetValue(ref _mainMediaPlayer, value); }
        }

        public StatusbarViewModel(ITranslationManager manager, MediaPlayers mediaPlayers)
        {
            _mediaplayers = mediaPlayers;

            _manager = manager;
            _manager.PropertyChanged += (o, e) =>
              {
                  if (e.PropertyName == $"{nameof(ITranslationManager.CurrentLanguage)}")
                      UpdateLanguage();
              };

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            Version = $"v{version.Major}.{version.Minor}.{version.Revision}";

            UpdateLanguage();

            MainMediaPlayer = (MainMediaPlayer)_mediaplayers.Items.ToList().Find(p => p is MainMediaPlayer);
        }

        private void UpdateLanguage()
        {
            Language = $"({_manager.CurrentLanguage.TwoLetterISOLanguageName})";
        }

        // TODO notifications?
    }
}
