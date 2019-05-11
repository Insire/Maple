using MvvmScarletToolkit;

namespace Maple.Core
{
    public class ViewModelSelectionChangingMessage<TViewModel> : GenericScarletMessage<TViewModel>
    {
        public ViewModelSelectionChangingMessage(object sender, TViewModel viewModel)
            : base(sender, viewModel)
        {
        }
    }
}
