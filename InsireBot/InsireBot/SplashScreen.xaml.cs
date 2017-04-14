using Maple.Core;

namespace Maple
{
    public partial class SplashScreen : IoCWindow
    {
        public SplashScreen(ITranslationService manager, UIColorsViewModel vm) : base(manager, vm)
        {
            InitializeComponent();
        }
    }
}
