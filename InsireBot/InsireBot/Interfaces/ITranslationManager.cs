﻿using Maple.Core;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Maple
{
    public interface ITranslationManager : INotifyPropertyChanged, IRefreshable
    {
        CultureInfo CurrentLanguage { get; set; }
        IEnumerable<CultureInfo> Languages { get; }
        string Translate(string key);
    }
}
