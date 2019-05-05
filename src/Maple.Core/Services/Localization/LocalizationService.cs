using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple.Core
{
    public sealed class LocalizationService : ObservableObject, ILocalizationService
    {
        public readonly ILoggingService _log;
        public ITranslationProvider TranslationProvider { get; }

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

        public LocalizationService(ITranslationProvider provider, ILoggingService log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log), $"{nameof(log)} {Resources.IsRequired}");
            TranslationProvider = provider ?? throw new ArgumentNullException(nameof(provider), $"{nameof(provider)} {Resources.IsRequired}");
            _currentLanguage = Thread.CurrentThread.CurrentUICulture;
        }

        public string Translate(string key)
        {
            if (TranslationProvider != null)
            {
                if (Thread.CurrentThread.CurrentUICulture != CurrentLanguage)
                    Thread.CurrentThread.CurrentUICulture = CurrentLanguage;

                var translatedValue = TranslationProvider.Translate(key);
                if (!string.IsNullOrEmpty(translatedValue))
                    return translatedValue;
            }

            return $"!{key}!";
        }

        public Task Save()
        {
            Properties.Settings.Default.StartUpCulture = CurrentLanguage;
            Properties.Settings.Default.Save();

            return Task.CompletedTask;
        }

        public Task Load()
        {
            _log.Info($"{Resources.Loading} {GetType().Name}");
            Thread.CurrentThread.CurrentCulture = Properties.Settings.Default.StartUpCulture;

            return Task.CompletedTask;
        }
    }
}
