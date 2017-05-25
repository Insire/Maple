using Maple.Core;
using System;

namespace Maple
{
    public partial class SplashScreen : IoCWindow
    {
        public SplashScreen(ILocalizationService manager, IUIColorsViewModel vm, ISplashScreenViewModel datacontext) : base(manager, vm)
        {
            DataContext = datacontext ?? throw new ArgumentNullException(nameof(datacontext));

            InitializeComponent();
        }
    }
}
