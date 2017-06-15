using Maple.Core;
using System;
using System.Linq;

namespace Maple
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Maple.Core.ObservableObject" />
    public class StatusbarViewModel : ObservableObject
    {
        private readonly ILocalizationService _manager;
        private readonly IMessenger _messenger;

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
        public StatusbarViewModel(ILocalizationService manager, IVersionService version, IMessenger messenger)
        {
            _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));
            _manager = manager ?? throw new ArgumentNullException(nameof(manager));

            _manager.PropertyChanged += (o, e) =>
              {
                  if (e.PropertyName == $"{nameof(ILocalizationService.CurrentLanguage)}")
                      UpdateLanguage();
              };

            Version = version.Get();

            UpdateLanguage();

            _messenger.Subscribe<IMapleMessage>(UpdateMainMediaPlayer);
        }

        private void UpdateLanguage()
        {
            Language = $"({_manager.CurrentLanguage.TwoLetterISOLanguageName})";
        }

        private void UpdateMainMediaPlayer(IMapleMessage obj)
        {
            //var main = obj.Sender as MainMediaPlayer;
            //MainMediaPlayer = (MainMediaPlayer)_mediaplayers.Items.ToList().Find(p => p is MainMediaPlayer);
        }

        // TODO notifications?
    }
}
