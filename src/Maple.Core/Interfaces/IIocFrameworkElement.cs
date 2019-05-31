using MvvmScarletToolkit.Abstractions;

namespace Maple.Core
{
    public interface IIocFrameworkElement
    {
        ILocalizationService LocalizationService { get; }
    }
}
