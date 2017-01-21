using System;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xaml;

namespace InsireBot
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
            var provider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
            var element = provider.RootObject as IIocFrameworkElement;

            if (element != null)
            {
                var binding = new Binding("Value")
                {
                    Source = new TranslationData(element.TranslationManager, _key)
                };

                return binding.ProvideValue(serviceProvider);
            }

            Debug.Fail($"ProvideValue {Key} failed");
            return null;
        }
    }
}
