using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public partial class OptionsPage
    {
        public OptionsPage(ILocalizationService manager)
            : base(manager)
        {
            InitializeComponent();
        }
    }
}
