using System.Windows.Input;

namespace Maple.Core
{
    public interface IUIColorsViewModel : ILoadableViewModel, ISaveableViewModel
    {
        ICommand ApplyAccentCommand { get; }
        ICommand ApplyPrimaryCommand { get; }
        ICommand ToggleBaseCommand { get; }

        void OnPrimaryColorChanged(UiPrimaryColorChangedMessage args);
    }
}