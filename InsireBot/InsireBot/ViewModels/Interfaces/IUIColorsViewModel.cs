using Maple.Core;
using System.Windows.Input;

namespace Maple
{
    public interface IUIColorsViewModel : ILoadableViewModel, ISaveableViewModel
    {
        ICommand ApplyAccentCommand { get; }
        ICommand ApplyPrimaryCommand { get; }
        ICommand ToggleBaseCommand { get; }

        event UiPrimaryColorEventHandler PrimaryColorChanged;

        void OnPrimaryColorChanged(UiPrimaryColorEventArgs args);
    }
}