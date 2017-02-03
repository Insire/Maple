namespace Maple
{
    /// <summary>
    /// SharedResourceDictionary to support <see cref="TranslationExtension"/>
    /// </summary>
    public class IoCResourceDictionary : SharedResourceDictionary, IIocFrameworkElement
    {
        public ITranslationManager TranslationManager { get; private set; }

        public IoCResourceDictionary() : base()
        {
            // for compatibility
        }

        public IoCResourceDictionary(ITranslationManager translationManager) : base()
        {
            TranslationManager = translationManager;
        }
    }
}
