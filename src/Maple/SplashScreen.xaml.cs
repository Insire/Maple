using System;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public partial class SplashScreen : IoCWindow
    {
        public SplashScreen(ILocalizationService manager, IScarletMessenger messenger, SplashScreenViewModel datacontext) : base(manager, messenger)
        {
            DataContext = datacontext ?? throw new ArgumentNullException(nameof(datacontext));

            InitializeComponent();
        }
    }
}
