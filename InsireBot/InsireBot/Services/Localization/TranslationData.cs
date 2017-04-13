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
        private string _key;
        private ITranslationService _manager;

        public TranslationData(ITranslationService manager, string key)
        {
            _manager = manager;
            _key = key;

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
            get { return _manager?.Translate(_key); }
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
    }
}
