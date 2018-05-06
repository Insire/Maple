using System;

using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple.Core
{
    public class StatusbarViewModel : ViewModel
    {
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
        public StatusbarViewModel(IVersionService version, IMessenger messenger)
            : base(messenger)
        {
            if (version == null)
                throw new ArgumentNullException(nameof(version), $"{nameof(version)} {Resources.IsRequired}");

            Version = version.Get();

            MessageTokens.Add(Messenger.Subscribe<ViewModelSelectionChangedMessage<Culture>>(UpdateLanguage));
            MessageTokens.Add(Messenger.Subscribe<ViewModelSelectionChangedMessage<MediaPlayer>>(UpdateMediaPlayer));
        }

        private void UpdateLanguage(ViewModelSelectionChangedMessage<Culture> message)
        {
            Language = $"({message.Content.Model.TwoLetterISOLanguageName})";
        }

        private void UpdateMediaPlayer(ViewModelSelectionChangedMessage<MediaPlayer> message)
        {
            MainMediaPlayer = message.Content as MainMediaPlayer;
        }

        // TODO add message queue and notify user about important notifications
    }
}
