using System;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public partial class SplashScreen : IoCWindow
    {
        public SplashScreen(ILocalizationService localizationService, IScarletCommandBuilder commandBuilder, SplashScreenViewModel datacontext)
            : base(commandBuilder, localizationService)
        {
            DataContext = datacontext ?? throw new ArgumentNullException(nameof(datacontext));

            InitializeComponent();
        }
    }
}
