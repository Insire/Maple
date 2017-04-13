using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Windows.Controls;

namespace Maple
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.Controls.UserControl" />
    /// <seealso cref="Maple.Core.IIocFrameworkElement" />
    public class IoCUserControl : UserControl, IIocFrameworkElement
    {
        /// <summary>
        /// Gets the translation manager.
        /// </summary>
        /// <value>
        /// The translation manager.
        /// </value>
        public ITranslationService TranslationManager { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IoCUserControl"/> class.
        /// </summary>
        public IoCUserControl() : base()
        {
            if (Debugger.IsAttached)
                Assert.Fail($"The constructor without parameters of {nameof(IoCUserControl)} exists only for compatibility reasons.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IoCUserControl"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public IoCUserControl(ITranslationService manager) : base()
        {
            TranslationManager = manager;
        }
    }
}
