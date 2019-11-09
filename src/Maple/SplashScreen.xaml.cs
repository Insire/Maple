using System;
using System.ComponentModel;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public partial class SplashScreen : IoCWindow
    {
        public SplashScreen(ILocalizationService manager, IScarletMessenger messenger, IScarletEventManager<INotifyPropertyChanged, PropertyChangedEventArgs> weakEventManager, SplashScreenViewModel datacontext)
            : base(manager, messenger, weakEventManager)
        {
            DataContext = datacontext ?? throw new ArgumentNullException(nameof(datacontext));

            InitializeComponent();
        }
    }
}
