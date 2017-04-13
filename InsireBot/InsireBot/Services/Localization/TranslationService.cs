using Maple.Core;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Maple
{
    public class TranslationService : ObservableObject, ITranslationService
    {
        public ITranslationProvider TranslationProvider { get; private set; }

        /// <summary>
        /// Gets or sets the current language.
        /// </summary>
        /// <value>
        /// The current language.
        /// </value>
        public CultureInfo CurrentLanguage
        {
            get { return Thread.CurrentThread.CurrentUICulture; }
            set
            {
                if (value != Thread.CurrentThread.CurrentUICulture)
                {
                    Thread.CurrentThread.CurrentUICulture = value;
                    OnPropertyChanged(nameof(CurrentLanguage));
                }
            }
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

        public TranslationService(ITranslationProvider provider)
        {
            TranslationProvider = provider;
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
            Thread.CurrentThread.CurrentCulture = Languages.First(p => p.TwoLetterISOLanguageName == "en");
        }
    }
}
