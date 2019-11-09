using System.ComponentModel;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public interface IIocFrameworkElement
    {
        ILocalizationService LocalizationService { get; }
        IScarletEventManager<INotifyPropertyChanged, PropertyChangedEventArgs> WeakEventManager { get; }
    }
}
