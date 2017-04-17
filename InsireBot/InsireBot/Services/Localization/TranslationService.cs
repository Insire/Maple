using Maple.Core;
using Maple.Localization.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Maple
{
    public class TranslationService : ObservableObject, ITranslationService
    {
        public ITranslationProvider TranslationProvider { get; private set; }
        public readonly IMapleLog _log;
        /// <summary>
        /// Gets or sets the current language.
        /// </summary>
        /// <value>
        /// The current language.
        /// </value>
        private CultureInfo _currentLanguage;
        public CultureInfo CurrentLanguage
        {
            get { return _currentLanguage; }
            set { SetValue(ref _currentLanguage, value, () => Thread.CurrentThread.CurrentUICulture = value); }
        }

        public IEnumerable<CultureInfo> Languages
        {
            get
            {
                if (TranslationProvider != null)
                    return TranslationProvider.Languages;

                return Enumerable.Empty<CultureInfo>();
            }
        }

        public TranslationService(ITranslationProvider provider, IMapleLog log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            TranslationProvider = provider ?? throw new ArgumentNullException(nameof(provider));
            _currentLanguage = Thread.CurrentThread.CurrentUICulture;
        }

        public string Translate(string key)
        {
            if (TranslationProvider != null)
            {
                var translatedValue = TranslationProvider.Translate(key);
                if (!string.IsNullOrEmpty(translatedValue))
                    return translatedValue;
            }

            return $"!{key}!";
        }

        public void Save()
        {
            Properties.Settings.Default.StartUpCulture = CurrentLanguage;
            Properties.Settings.Default.Save();
        }

        public void Load()
        {
            _log.Info($"{Resources.Loading} {GetType().Name}");
            Thread.CurrentThread.CurrentCulture = Properties.Settings.Default.StartUpCulture;
        }

        public Task SaveAsync()
        {
            Save();
            return Task.FromResult(0);
        }

        public Task LoadAsync()
        {
            Load();
            return Task.FromResult(0);
        }
    }
}
