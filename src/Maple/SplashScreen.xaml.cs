using System;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public partial class SplashScreen : IoCWindow
    {
        public SplashScreen(ILocalizationService manager, IScarletMessenger messenger, ISplashScreenViewModel datacontext) : base(manager, messenger)
        {
            DataContext = datacontext ?? throw new ArgumentNullException(nameof(datacontext), $"{nameof(datacontext)} {Localization.Properties.Resources.IsRequired}");

            InitializeComponent();
        }
    }
}
