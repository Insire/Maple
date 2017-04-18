using Maple.Core;
using System;
using System.ComponentModel;
using System.Windows;

namespace Maple
{
    /// <summary>
    /// Poco that holds localization data and supports changing it during runtime
    /// </summary>
    /// <seealso cref="Maple.Core.ObservableObject" />
    /// <seealso cref="System.Windows.IWeakEventListener" />
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    /// <seealso cref="System.IDisposable" />
    public class TranslationData : ObservableObject, IWeakEventListener, INotifyPropertyChanged, IDisposable
    {
        private readonly string _key;
        private readonly ITranslationService _manager;
        private readonly bool _toUpper;

        public TranslationData(ITranslationService manager, string key, bool toUpper)
        {
            _manager = manager ?? throw new ArgumentNullException(nameof(manager));
            _key = key ?? throw new ArgumentNullException(nameof(key));
            _toUpper = toUpper;

            LanguageChangedEventManager.AddListener(_manager, this);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                LanguageChangedEventManager.RemoveListener(_manager, this);
        }

        public object Value
        {
            get { return GetValue(); }
        }

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (managerType == typeof(LanguageChangedEventManager))
            {
                OnPropertyChanged(nameof(Value));
                return true;
            }
            return false;
        }

        private string GetValue()
        {
            if (_toUpper)
                return _manager?.Translate(_key).ToUpperInvariant();

            return _manager?.Translate(_key);
        }
    }
}
