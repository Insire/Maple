using System;
using System.ComponentModel;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public partial class SplashScreen : IoCWindow
    {
        public SplashScreen(ILocalizationService manager, IScarletMessenger messenger, IScarletEventManager<INotifyPropertyChanged, PropertyChangedEventArgs> weakEventManager, IScarletCommandBuilder commandBuilder, SplashScreenViewModel datacontext)
            : base(manager, messenger, weakEventManager, commandBuilder)
        {
            DataContext = datacontext ?? throw new ArgumentNullException(nameof(datacontext));

            InitializeComponent();
        }
    }
}
