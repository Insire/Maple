using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xaml;
using MvvmScarletToolkit.Abstractions;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    [ValueConversion(typeof(string), typeof(string))]
    public sealed class TranslationExtension : MarkupExtension
    {
        public TranslationExtension(string key)
        {
            Key = key;
        }

        [ConstructorArgument("key")]
        public string Key { get; set; }

        [ConstructorArgument("ToUpper")]
        public bool ToUpper { get; set; }

        /// <summary>
        /// When implemented in a derived class, returns an object that is provided as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>
        /// The object value to set on the property where the extension is applied.
        /// </returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (TryGetIoCFrameWorkElement(serviceProvider, out var element))
                return ProvideValue(serviceProvider, element.WeakEventManager, element.LocalizationService);

            if (TryGetServiceFromResources<IScarletEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>>(serviceProvider, out var manager) && TryGetServiceFromResources<ILocalizationService>(serviceProvider, out var service))
                return ProvideValue(serviceProvider, manager, service);

            Debug.WriteLine($"{nameof(TranslationExtension)} ProvideValue {Key} failed");

            return Binding.DoNothing;
        }

        private static bool TryGetIoCFrameWorkElement(IServiceProvider serviceProvider, out IIocFrameworkElement element)
        {
            var provider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
            element = provider?.RootObject as IIocFrameworkElement;
            return element != null;
        }

        private static bool TryGetServiceFromResources<T>(IServiceProvider serviceProvider, out T service)
            where T : class
        {
            var provider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
            var dictionary = provider?.RootObject as ResourceDictionary;
            var key = typeof(T).Name;

            if (dictionary?.Contains(key) == true)
            {
                service = dictionary[key] as T;
                return true;
            }

            service = null;
            return false;
        }

        private object ProvideValue(IServiceProvider serviceProvider, IScarletEventManager<INotifyPropertyChanged, PropertyChangedEventArgs> weakEventManager, ILocalizationService service)
        {
            var binding = new Binding("Value")
            {
                Source = new LocalizationViewModel(weakEventManager, service, Key, ToUpper)
            };

            return binding.ProvideValue(serviceProvider);
        }
    }
}
