using InsireBot.Core;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace InsireBot
{
    public interface ITranslationManager : INotifyPropertyChanged, ISaveable
    {
        CultureInfo CurrentLanguage { get; set; }
        IEnumerable<CultureInfo> Languages { get; }
        string Translate(string key);
    }
}
