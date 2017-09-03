using System.Collections.ObjectModel;

namespace Maple.Core
{
    public class ViewModelSelectionChangedMessage<TViewModel> : GenericMapleMessage<TViewModel>
    {
        public ViewModelSelectionChangedMessage(ObservableCollection<TViewModel> sender, TViewModel viewModel) : base(sender, viewModel)
        {
        }
    }
}
