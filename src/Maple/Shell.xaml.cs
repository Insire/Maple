using Jot;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public partial class Shell : IoCWindow
    {
        public Shell(ILocalizationService localizationService, IScarletCommandBuilder commandBuilder, Tracker tracker)
            : base(commandBuilder, localizationService)
        {
            InitializeComponent();

            tracker.Track(this);
        }
    }
}
