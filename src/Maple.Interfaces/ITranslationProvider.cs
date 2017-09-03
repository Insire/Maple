using System.Collections.Generic;
using System.Globalization;

namespace Maple.Interfaces
{
    public interface ITranslationProvider
    {
        /// <summary>
        /// Translates the key into a localized value
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        string Translate(string key);

        /// <summary>
        /// Gets the available languages.
        /// </summary>
        /// <value>The available languages.</value>
        IEnumerable<CultureInfo> Languages { get; }
    }
}
