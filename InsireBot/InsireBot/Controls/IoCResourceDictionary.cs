using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Maple
{
    /// <summary>
    /// SharedResourceDictionary to support <see cref="TranslationExtension" />
    /// </summary>
    /// <seealso cref="Maple.SharedResourceDictionary" />
    /// <seealso cref="Maple.Core.IIocFrameworkElement" />
    public class IoCResourceDictionary : SharedResourceDictionary, IIocFrameworkElement
    {
        /// <summary>
        /// Gets the translation manager.
        /// </summary>
        /// <value>
        /// The translation manager.
        /// </value>
        public ITranslationService TranslationManager { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IoCResourceDictionary"/> class.
        /// </summary>
        public IoCResourceDictionary() : base()
        {
            Assert.Fail($"The constructor without parameters of {nameof(IoCResourceDictionary)} exists only for compatibility reasons.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IoCResourceDictionary"/> class.
        /// </summary>
        /// <param name="translationManager">The translation manager.</param>
        public IoCResourceDictionary(ITranslationService translationManager) : base()
        {
            TranslationManager = translationManager;
        }
    }
}
