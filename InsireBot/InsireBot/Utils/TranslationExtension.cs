using Maple.Core;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xaml;

namespace Maple
{
    public class TranslationExtension : MarkupExtension
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
            if (TryGetIoCFrameWorkElement(serviceProvider, out IIocFrameworkElement element))
                return ProvideValue(serviceProvider, element.TranslationManager);

            if (TryGetTranslationManagerFromResources(serviceProvider, out ILocalizationService manager))
                return ProvideValue(serviceProvider, manager);

            Debug.WriteLine($"{nameof(TranslationExtension)} ProvideValue {Key} failed");

            if (Debugger.IsAttached)
                Debug.Fail($"{nameof(TranslationExtension)} ProvideValue {Key} failed");

            return null;
        }

        private bool TryGetIoCFrameWorkElement(IServiceProvider serviceProvider, out IIocFrameworkElement element)
        {
            var provider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
            element = provider?.RootObject as IIocFrameworkElement;
            return element != null;
        }

        private bool TryGetTranslationManagerFromResources(IServiceProvider serviceProvider, out ILocalizationService manager)
        {
            var provider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
            var dictionary = provider?.RootObject as ResourceDictionary;
            var key = typeof(ILocalizationService).Name;

            if (dictionary?.Contains(key) == true)
            {
                manager = dictionary[key] as ILocalizationService;
                return true;
            }

            manager = null;
            return false;
        }

        private object ProvideValue(IServiceProvider serviceProvider, ILocalizationService manager)
        {
            var binding = new Binding("Value")
            {
                Source = new LocalizationDTO(manager, Key, ToUpper)
            };

            return binding.ProvideValue(serviceProvider);
        }
    }
}
