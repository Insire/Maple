using Maple.Domain;

namespace Maple.Core
{
    public interface IIocFrameworkElement
    {
        ILocalizationService TranslationManager { get; }
    }
}
