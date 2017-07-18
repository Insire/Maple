namespace Maple.Core
{
    public class LoadedMessage : GenericMapleMessage<ObservableObject>
    {
        public LoadedMessage(object sender, ObservableObject viewModel) : base(sender, viewModel)
        {
        }
    }
}
