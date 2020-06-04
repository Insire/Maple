using System;
using System.ComponentModel;
using System.Windows;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public sealed class IoCResourceDictionary : ResourceDictionary, IIocFrameworkElement
    {
        public ILocalizationService LocalizationService { get; }

        public IScarletEventManager<INotifyPropertyChanged, PropertyChangedEventArgs> WeakEventManager { get; }

        public IoCResourceDictionary(ILocalizationService localizationService, IScarletEventManager<INotifyPropertyChanged, PropertyChangedEventArgs> weakEventManager, Uri source)
            : base()
        {
            LocalizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
            WeakEventManager = weakEventManager ?? throw new ArgumentNullException(nameof(weakEventManager));
            Source = source ?? throw new ArgumentNullException(nameof(source));

            Add(typeof(IScarletEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>).Name, weakEventManager);
            Add(typeof(ILocalizationService).Name, localizationService);
        }
    }
}
