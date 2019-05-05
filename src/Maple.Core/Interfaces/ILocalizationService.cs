using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

using Maple.Domain;

namespace Maple.Core
{
    public interface ILocalizationService : INotifyPropertyChanged, IRefreshable
    {
        CultureInfo CurrentLanguage { get; set; }

        IEnumerable<CultureInfo> Languages { get; }

        string Translate(string key);
    }
}
