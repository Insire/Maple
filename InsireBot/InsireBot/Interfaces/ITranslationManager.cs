using System;
using System.Collections.Generic;
using System.Globalization;

namespace InsireBot
{
    public interface ITranslationManager
    {
        event EventHandler LanguageChanged;
        CultureInfo CurrentLanguage { get; set; }
        IEnumerable<CultureInfo> Languages { get; }
        string Translate(string key);
    }
}
