using System;
using System.Threading;
using System.Threading.Tasks;
using Maple.Core;
using Maple.Domain;

namespace Maple
{
    public class StatusbarViewModel : MapleBusinessViewModelBase
    {
        private readonly IVersionService _versionService;

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
        public StatusbarViewModel(IMapleCommandBuilder commandBuilder, IVersionService versionService)
            : base(commandBuilder)
        {
            _versionService = versionService ?? throw new ArgumentNullException(nameof(versionService));
        }

        private void UpdateLanguage(ViewModelSelectionChangedMessage<Culture> message)
        {
            Language = $"({message.Content.Model.TwoLetterISOLanguageName})";
        }

        private void UpdateMediaPlayer(ViewModelSelectionChangedMessage<MediaPlayer> message)
        {
            MainMediaPlayer = message.Content as MainMediaPlayer;
        }

        protected override Task UnloadInternal(CancellationToken token)
        {
            ClearSubscriptions();
            Version = string.Empty;

            return Task.CompletedTask;
        }

        protected override Task RefreshInternal(CancellationToken token)
        {
            Version = _versionService.Get();

            Add(Messenger.Subscribe<ViewModelSelectionChangedMessage<Culture>>(UpdateLanguage));
            Add(Messenger.Subscribe<ViewModelSelectionChangedMessage<MediaPlayer>>(UpdateMediaPlayer));

            return Task.CompletedTask;
        }

        // TODO add message queue and notify user about important notifications
    }
}
