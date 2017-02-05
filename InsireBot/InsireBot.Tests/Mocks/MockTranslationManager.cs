using Maple.Core;
using System.Collections.Generic;
using System.Globalization;

namespace Maple.Tests
{
    public class MockTranslationManager : ObservableObject, ITranslationManager
    {
        public CultureInfo CurrentLanguage { get; set; }

        public IEnumerable<CultureInfo> Languages { get; set; }

        public void Save()
        {
        }

        public string Translate(string key)
        {
            return "MockTranslation";
        }
    }
}
