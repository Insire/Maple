using System.ComponentModel;
using Jot;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public partial class Shell : IoCWindow
    {
        public Shell(ILocalizationService manager, IScarletMessenger messenger, IScarletEventManager<INotifyPropertyChanged, PropertyChangedEventArgs> weakEventManager, IScarletCommandBuilder commandBuilder, Tracker tracker)
            : base(manager, messenger, weakEventManager, commandBuilder)
        {
            InitializeComponent();

            tracker.Track(this);
        }
    }
}
