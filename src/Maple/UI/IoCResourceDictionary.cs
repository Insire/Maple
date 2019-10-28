using System;
using System.ComponentModel;
using System.Diagnostics;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    /// <summary>
    /// SharedResourceDictionary to support <see cref="TranslationExtension" />
    /// </summary>
    /// <seealso cref="Maple.SharedResourceDictionary" />
    /// <seealso cref="Maple.Core.IIocFrameworkElement" />
    public class IoCResourceDictionary : SharedResourceDictionary, IIocFrameworkElement
    {
        public ILocalizationService LocalizationService { get; }

        public IWeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs> WeakEventManager { get; }

        public IoCResourceDictionary()
            : base()
        {
            if (Debugger.IsAttached)
                Debug.Fail($"The constructor without parameters of {nameof(IoCResourceDictionary)} exists only for compatibility reasons.");
        }

        public IoCResourceDictionary(ILocalizationService localizationService, IWeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs> weakEventManager, Uri source)
            : base()
        {
            LocalizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
            WeakEventManager = weakEventManager ?? throw new ArgumentNullException(nameof(weakEventManager));
            Source = source ?? throw new ArgumentNullException(nameof(source));

            Add(typeof(ILocalizationService).Name, localizationService);
        }
    }
}
