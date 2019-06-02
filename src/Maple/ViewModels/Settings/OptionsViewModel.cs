using System;
using Maple.Localization.Properties;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    public class OptionsViewModel : ObservableObject
    {
        private Cultures _cultureViewModel;
        public Cultures CultureViewModel
        {
            get { return _cultureViewModel; }
            private set { SetValue(ref _cultureViewModel, value); }
        }

        public OptionsViewModel(Cultures cultures)
        {
            CultureViewModel = cultures ?? throw new ArgumentNullException(nameof(cultures));
        }
    }
}
