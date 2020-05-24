using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public class ResxTranslationProvider : ILocalizationProvider
    {
        private readonly ResourceManager _resourceManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResxTranslationProvider"/> class.
        /// </summary>
        /// <param name="baseName">Name of the base.</param>
        /// <param name="assembly">The assembly that provides localized strings</param>
        public ResxTranslationProvider()
        {
            _resourceManager = new ResourceManager(typeof(Properties.Resources));
            Languages = new CultureInfo[]
            {
                new CultureInfo("de"),
                new CultureInfo("en")
            };
        }

        public string Translate(string key, CultureInfo culture)
        {
            return _resourceManager.GetString(key, culture);
        }

        public IEnumerable<CultureInfo> Languages { get; }
    }
}
