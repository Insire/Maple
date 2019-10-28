using System.ComponentModel;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public interface IIocFrameworkElement
    {
        ILocalizationService LocalizationService { get; }
        IWeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs> WeakEventManager { get; }
    }
}
