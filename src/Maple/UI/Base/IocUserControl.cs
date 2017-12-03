using System.Diagnostics;
using System.Windows.Controls;
using Maple.Core;

namespace Maple
{
    public abstract class IoCUserControl : UserControl, IIocFrameworkElement
    {
        public ILocalizationService TranslationManager { get; private set; }

        protected IoCUserControl() : base()
        {
            if (Debugger.IsAttached)
                Debug.Fail($"The constructor without parameters of {nameof(IoCUserControl)} exists only for compatibility reasons.");
        }

        protected IoCUserControl(ILocalizationService manager) : base()
        {
            TranslationManager = manager;
        }
    }
}
