using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;

namespace Maple.Domain
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    /// <seealso cref="Maple.Core.IRefreshable" />
    public interface ILocalizationService : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the current language.
        /// </summary>
        /// <value>
        /// The current language.
        /// </value>
        CultureInfo CurrentLanguage { get; set; }
        /// <summary>
        /// Gets the languages.
        /// </summary>
        /// <value>
        /// The languages.
        /// </value>
        IEnumerable<CultureInfo> Languages { get; }
        /// <summary>
        /// Translates the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        string Translate(string key);

        Task Save();
        Task LoadAsync();
    }
}
