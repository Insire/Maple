using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Windows.Controls;

namespace Maple
{
    public class IoCUserControl : UserControl, IIocFrameworkElement
    {
        public ITranslationManager TranslationManager { get; private set; }

        public IoCUserControl() : base()
        {
            if (Debugger.IsAttached)
                Assert.Fail($"The constructor without parameters of {nameof(IoCUserControl)} exists only for compatibility reasons.");
        }

        public IoCUserControl(ITranslationManager manager) : base()
        {
            TranslationManager = manager;
        }
    }
}
