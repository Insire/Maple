using MahApps.Metro.Controls;

namespace InsireBot
{
    public class IoCWindow : MetroWindow, IIocFrameworkElement
    {
        public ITranslationManager TranslationManager { get; private set; }

        public IoCWindow(ITranslationManager container) : base()
        {
            TranslationManager = container;
        }
    }
}
