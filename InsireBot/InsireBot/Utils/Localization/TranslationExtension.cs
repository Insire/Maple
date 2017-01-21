using DryIoc;
using System;
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
            var window = provider.RootObject as IoCWindow;

            if (window != null)
            {
                //var manager = GlobalServiceLocator.Instance.TranslationManager;
                var manager = window.Container.Resolve<ITranslationManager>();
                if (manager != null)
                {
                    var binding = new Binding("Value")
                    {
                        Source = new TranslationData(manager, _key)
                    };
                    return binding.ProvideValue(serviceProvider);
                }
            }
            return null;
        }
    }
}
