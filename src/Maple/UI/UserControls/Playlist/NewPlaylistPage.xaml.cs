using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public partial class NewPlaylistPage
    {
        public NewPlaylistPage(ILocalizationService manager)
            : base(manager)
        {
            InitializeComponent();
        }
    }
}
