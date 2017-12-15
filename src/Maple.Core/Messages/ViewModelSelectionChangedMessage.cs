using System.Collections.Generic;

namespace Maple.Core
{
    public class ViewModelSelectionChangedMessage<TViewModel> : GenericMapleMessage<TViewModel>
    {
        public ViewModelSelectionChangedMessage(IReadOnlyCollection<TViewModel> sender, TViewModel viewModel) : base(sender, viewModel)
        {
        }
    }
}
