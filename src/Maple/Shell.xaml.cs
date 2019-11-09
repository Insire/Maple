using System.ComponentModel;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public partial class Shell : IoCWindow
    {
        public Shell(ILocalizationService manager, IScarletMessenger messenger, IScarletEventManager<INotifyPropertyChanged, PropertyChangedEventArgs> weakEventManager)
            : base(manager, messenger, weakEventManager)
        {
            InitializeComponent();
        }
    }
}
