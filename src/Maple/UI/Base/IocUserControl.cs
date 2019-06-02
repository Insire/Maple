using System;
using System.Diagnostics;
using System.Windows.Controls;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public abstract class IoCUserControl : UserControl, IIocFrameworkElement
    {
        public ILocalizationService LocalizationService { get; }

        protected IoCUserControl() : base()
        {
            if (Debugger.IsAttached)
                Debug.Fail($"The constructor without parameters of {nameof(IoCUserControl)} exists only for compatibility reasons.");
        }

        protected IoCUserControl(ILocalizationService localizationService) : base()
        {
            LocalizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
        }
    }
}
