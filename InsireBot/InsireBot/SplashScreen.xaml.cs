using Maple.Core;

namespace Maple
{
    public partial class SplashScreen : IoCWindow
    {
        public SplashScreen(ITranslationService manager, IUIColorsViewModel vm) : base(manager, vm)
        {
            InitializeComponent();
        }
    }
}
