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

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (TryGetIoCFrameWorkElement(serviceProvider, out IIocFrameworkElement element))
                return ProvideValue(serviceProvider, element.TranslationManager);

            if (TryGetTranslationManagerFromResources(serviceProvider, out ITranslationService manager))
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

        private bool TryGetTranslationManagerFromResources(IServiceProvider serviceProvider, out ITranslationService manager)
        {
            var provider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
            var dictionary = provider?.RootObject as ResourceDictionary;
            var key = typeof(ITranslationService).Name;

            if (dictionary?.Contains(key) == true)
            {
                manager = dictionary[key] as ITranslationService;
                return true;
            }

            manager = null;
            return false;
        }

        private object ProvideValue(IServiceProvider serviceProvider, ITranslationService manager)
        {
            var binding = new Binding("Value")
            {
                Source = new TranslationData(manager, Key)
            };

            return binding.ProvideValue(serviceProvider);
        }
    }
}
