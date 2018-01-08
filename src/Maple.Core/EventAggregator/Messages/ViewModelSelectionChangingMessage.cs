namespace Maple.Core
{
    public class ViewModelSelectionChangingMessage<TViewModel> : GenericMapleMessage<TViewModel>
    {
        public ViewModelSelectionChangingMessage(object sender, TViewModel viewModel) : base(sender, viewModel)
        {
        }
    }
}
