using MvvmScarletToolkit;

namespace Maple
{
    public class ViewModelSelectionChangingMessage<TViewModel> : GenericScarletMessage<TViewModel>
    {
        public ViewModelSelectionChangingMessage(object sender, TViewModel viewModel)
            : base(sender, viewModel)
        {
        }
    }
}
