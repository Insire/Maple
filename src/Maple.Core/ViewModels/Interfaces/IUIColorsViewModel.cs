using System.Windows.Input;

namespace Maple.Core
{
    public interface IUIColorsViewModel : ILoadableViewModel
    {
        ICommand ApplyAccentCommand { get; }
        ICommand ApplyPrimaryCommand { get; }
        ICommand ToggleBaseCommand { get; }

        void OnPrimaryColorChanged(UiPrimaryColorChangedMessage args);
    }
}