using System.Collections.Generic;
using MvvmScarletToolkit;

namespace Maple
{
    public class ViewModelSelectionChangedMessage<TViewModel> : GenericScarletMessage<TViewModel>
    {
        public ViewModelSelectionChangedMessage(IReadOnlyCollection<TViewModel> sender, TViewModel viewModel)
            : base(sender, viewModel)
        {
        }
    }
}
