using System;
using Maple.Core;
using Maple.Interfaces;
using Maple.Localization.Properties;

namespace Maple
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Maple.Core.ObservableObject" />
    public class StatusbarViewModel : ObservableObject
    {
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
        public StatusbarViewModel(IVersionService version, IMessenger messenger)
        {
            _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger), $"{nameof(messenger)} {Resources.IsRequired}");

            Version = version.Get();

            _messenger.Subscribe<ViewModelSelectionChangedMessage<Culture>>(UpdateLanguage);
            _messenger.Subscribe<ViewModelSelectionChangedMessage<MediaPlayer>>(UpdateMediaPlayer);
        }

        private void UpdateLanguage(ViewModelSelectionChangedMessage<Culture> message)
        {
            Language = $"({message.Content.Model.TwoLetterISOLanguageName})";
        }

        private void UpdateMediaPlayer(ViewModelSelectionChangedMessage<MediaPlayer> message)
        {
            MainMediaPlayer = message.Content as MainMediaPlayer;
        }

        // TODO notifications?
    }
}
