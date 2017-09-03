using Maple.Interfaces;

namespace Maple.Core
{
    public class ViewModelSelectionChangedMessage<TViewModel> : GenericMapleMessage<TViewModel>
    {
        public ViewModelSelectionChangedMessage(IRangeObservableCollection<TViewModel> sender, TViewModel viewModel) : base(sender, viewModel)
        {
        }
    }
}
