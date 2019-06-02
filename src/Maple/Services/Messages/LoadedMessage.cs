using System.ComponentModel;
using MvvmScarletToolkit;

namespace Maple
{
    public class LoadedMessage : GenericScarletMessage<INotifyPropertyChanged>
    {
        public LoadedMessage(object sender, INotifyPropertyChanged viewModel)
            : base(sender, viewModel)
        {
        }
    }
}
