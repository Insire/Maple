namespace Maple.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IIocFrameworkElement
    {
        /// <summary>
        /// Gets the translation manager.
        /// </summary>
        /// <value>
        /// The translation manager.
        /// </value>
        ITranslationService TranslationManager { get; }
    }
}
