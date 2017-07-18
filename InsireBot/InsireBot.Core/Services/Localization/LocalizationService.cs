﻿using Maple.Localization.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Maple.Core
{
    public class LocalizationService : ObservableObject, ILocalizationService
    {
        public ITranslationProvider TranslationProvider { get; private set; }
        public readonly ILoggingService _log;
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

        public LocalizationService(ITranslationProvider provider, ILoggingService log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            TranslationProvider = provider ?? throw new ArgumentNullException(nameof(provider));
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
            return Task.CompletedTask;
        }

        public Task LoadAsync()
        {
            Load();
            return Task.CompletedTask;
        }
    }
}
