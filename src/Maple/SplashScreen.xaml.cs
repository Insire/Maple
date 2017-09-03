using Maple.Core;
using System;

namespace Maple
{
    public partial class SplashScreen : IoCWindow
    {
        public SplashScreen(ILocalizationService manager, IMessenger messenger, ISplashScreenViewModel datacontext) : base(manager, messenger)
        {
            DataContext = datacontext ?? throw new ArgumentNullException(nameof(datacontext), $"{nameof(datacontext)} {Localization.Properties.Resources.IsRequired}");

            InitializeComponent();
        }
    }
}
