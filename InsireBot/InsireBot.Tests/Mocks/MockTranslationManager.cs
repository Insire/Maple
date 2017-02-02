using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace InsireBot.Tests
{
    public class MockTranslationManager : ITranslationManager
    {
        public CultureInfo CurrentLanguage { get; set; }

        public IEnumerable<CultureInfo> Languages { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Save()
        {
        }

        public string Translate(string key)
        {
            return "MockTranslation";
        }
    }
}
