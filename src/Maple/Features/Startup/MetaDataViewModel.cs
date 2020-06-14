using System;
using Maple.Domain;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    public sealed class MetaDataViewModel : ViewModelBase
    {
        private readonly IVersionService _versionService;

        private IDisposable _token;
        private bool _disposed;

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
        public MetaDataViewModel(IScarletCommandBuilder commandBuilder, IVersionService versionService)
            : base(commandBuilder)
        {
            _versionService = versionService ?? throw new ArgumentNullException(nameof(versionService));

            _token = Messenger.Subscribe<ViewModelListBaseSelectionChanged<Culture>>(UpdateLanguage);
            Version = _versionService.Get();
        }

        private void UpdateLanguage(ViewModelListBaseSelectionChanged<Culture> message)
        {
            Language = $"({message.Content.Model.TwoLetterISOLanguageName})";
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            if (disposing)
            {
                if (_token != null)
                {
                    _token?.Dispose();
                    _token = null;
                }
            }

            base.Dispose(disposing);
        }

        // TODO add message queue and notify user about important notifications
    }
}
