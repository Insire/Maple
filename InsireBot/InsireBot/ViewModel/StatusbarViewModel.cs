using Maple.Core;
using System.Linq;
using System.Reflection;

namespace Maple
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Maple.Core.ObservableObject" />
    public class StatusbarViewModel : ObservableObject
    {
        private readonly MediaPlayers _mediaplayers;
        private readonly ITranslationService _manager;

        private string _version;
        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string Version
        {
            get { return _version; }
            private set { SetValue(ref _version, value); }
        }

        private string _language;
        /// <summary>
        /// Gets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        public string Language
        {
            get { return _language; }
            private set { SetValue(ref _language, value); }
        }

        private MainMediaPlayer _mainMediaPlayer;
        /// <summary>
        /// Gets the main media player.
        /// </summary>
        /// <value>
        /// The main media player.
        /// </value>
        public MainMediaPlayer MainMediaPlayer
        {
            get { return _mainMediaPlayer; }
            private set { SetValue(ref _mainMediaPlayer, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusbarViewModel"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="mediaPlayers">The media players.</param>
        public StatusbarViewModel(ITranslationService manager, MediaPlayers mediaPlayers)
        {
            _mediaplayers = mediaPlayers;

            _manager = manager;
            _manager.PropertyChanged += (o, e) =>
              {
                  if (e.PropertyName == $"{nameof(ITranslationService.CurrentLanguage)}")
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
