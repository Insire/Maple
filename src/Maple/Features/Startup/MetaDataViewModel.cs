using System;
using System.Threading;
using System.Threading.Tasks;
using Maple.Domain;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    public sealed class MetaDataViewModel : MapleBusinessViewModelBase
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

        /// <summary>
        /// Initializes a new instance of the <see cref="MetaDataViewModel"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="mediaPlayers">The media players.</param>
        public MetaDataViewModel(IMapleCommandBuilder commandBuilder, IVersionService versionService)
            : base(commandBuilder)
        {
            _versionService = versionService ?? throw new ArgumentNullException(nameof(versionService));
        }

        private void UpdateLanguage(ViewModelListBaseSelectionChanged<Culture> message)
        {
            Language = $"({message.Content.Model.TwoLetterISOLanguageName})";
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

            Add(Messenger.Subscribe<ViewModelListBaseSelectionChanged<Culture>>(UpdateLanguage));

            return Task.CompletedTask;
        }

        // TODO add message queue and notify user about important notifications
    }
}
