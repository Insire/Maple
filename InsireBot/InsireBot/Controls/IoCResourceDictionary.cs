using Maple.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Maple
{
    /// <summary>
    /// SharedResourceDictionary to support <see cref="TranslationExtension"/>
    /// </summary>
    public class IoCResourceDictionary : SharedResourceDictionary, IIocFrameworkElement
    {
        public ITranslationService TranslationManager { get; private set; }

        public IoCResourceDictionary() : base()
        {
            Assert.Fail($"The constructor without parameters of {nameof(IoCResourceDictionary)} exists only for compatibility reasons.");
        }

        public IoCResourceDictionary(ITranslationService translationManager) : base()
        {
            TranslationManager = translationManager;
        }
    }
}
