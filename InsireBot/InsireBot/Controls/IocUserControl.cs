using System.Windows.Controls;

namespace InsireBot
{
    public class IoCUserControl : UserControl, IIocFrameworkElement
    {
        public ITranslationManager TranslationManager { get; private set; }

        public IoCUserControl() : base()
        {
        }

        public IoCUserControl(ITranslationManager manager) : base()
        {
            TranslationManager = manager;
        }
    }
}
