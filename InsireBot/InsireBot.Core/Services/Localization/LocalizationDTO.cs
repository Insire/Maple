using System;
using System.ComponentModel;
using System.Windows;

namespace Maple.Core
{
    /// <summary>
    /// Poco that holds localization data and supports changing it during runtime
    /// </summary>
    /// <seealso cref="Maple.Core.ObservableObject" />
    /// <seealso cref="System.Windows.IWeakEventListener" />
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    /// <seealso cref="System.IDisposable" />
    public class LocalizationDTO : ObservableObject, INotifyPropertyChanged, IDisposable
    {
        private readonly string _key;
        private readonly ILocalizationService _service;
        private readonly bool _toUpper;

        public LocalizationDTO(ILocalizationService service, string key, bool toUpper)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _key = key ?? throw new ArgumentNullException(nameof(key));
            _toUpper = toUpper;

            WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.AddHandler(_service, "PropertyChanged", ValueChanged);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.RemoveHandler(_service, "PropertyChanged", ValueChanged);
        }

        public object Value
        {
            get { return GetValue(); }
        }

        private void ValueChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Value));
        }

        private string GetValue()
        {
            if (_toUpper)
                return _service?.Translate(_key).ToUpperInvariant();

            return _service?.Translate(_key);
        }
    }
}
