using Maple.Core;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public partial class PlaylistsPage
    {
        public PlaylistsPage(ILocalizationService manager)
            : base(manager)
        {
            InitializeComponent();
        }
    }
}
