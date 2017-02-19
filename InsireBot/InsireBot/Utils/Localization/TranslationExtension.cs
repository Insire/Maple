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
            _key = key;
        }

        private string _key;

        [ConstructorArgument("key")]
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var element = default(IIocFrameworkElement);
            var manager = default(ITranslationManager);

            if (TryGetIoCFrameWorkElement(serviceProvider, out element))
                return ProvideValue(serviceProvider, element.TranslationManager);

            if (TryGetTranslationManagerFromResources(serviceProvider, out manager))
                return ProvideValue(serviceProvider, manager);

            Debug.Fail($"{nameof(TranslationExtension)} ProvideValue {Key} failed");
            Debug.WriteLine($"{nameof(TranslationExtension)} ProvideValue {Key} failed");
            return null;
        }

        private bool TryGetIoCFrameWorkElement(IServiceProvider serviceProvider, out IIocFrameworkElement element)
        {
            var provider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
            element = provider.RootObject as IIocFrameworkElement;
            return element != null;
        }

        private bool TryGetTranslationManagerFromResources(IServiceProvider serviceProvider, out ITranslationManager manager)
        {
            var provider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
            var dictionary = provider.RootObject as ResourceDictionary;
            var key = typeof(ITranslationManager).Name;

            if (dictionary?.Contains(key) == true)
            {
                manager = dictionary[key] as ITranslationManager;
                return true;
            }

            manager = null;
            return false;
        }

        private object ProvideValue(IServiceProvider serviceProvider, ITranslationManager manager)
        {
            var binding = new Binding("Value")
            {
                Source = new TranslationData(manager, _key)
            };

            return binding.ProvideValue(serviceProvider);
        }
    }
}
