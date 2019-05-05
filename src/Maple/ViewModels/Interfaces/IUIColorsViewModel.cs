using System.Windows.Input;

using Maple.Core;

namespace Maple
{
    public interface IUIColorsViewModel : ILoadableViewModel, ISaveableViewModel
    {
        ICommand ApplyAccentCommand { get; }
        ICommand ApplyPrimaryCommand { get; }
        ICommand ToggleBaseCommand { get; }

        void OnPrimaryColorChanged(UiPrimaryColorChangedMessage args);
    }
}
