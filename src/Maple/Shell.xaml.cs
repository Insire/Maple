using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public partial class Shell : IoCWindow
    {
        public Shell(ILocalizationService manager, IScarletMessenger messenger)
            : base(manager, messenger)
        {
            InitializeComponent();
        }
    }
}
