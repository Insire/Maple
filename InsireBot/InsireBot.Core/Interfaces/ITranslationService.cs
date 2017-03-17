using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Maple.Core
{
    public interface ITranslationService : INotifyPropertyChanged, IRefreshable
    {
        CultureInfo CurrentLanguage { get; set; }
        IEnumerable<CultureInfo> Languages { get; }
        string Translate(string key);
    }
}
