using System;
using Maple.Localization.Properties;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    public class OptionsViewModel : ObservableObject
    {
        private ICultureViewModel _cultureViewModel;
        public ICultureViewModel CultureViewModel
        {
            get { return _cultureViewModel; }
            set { SetValue(ref _cultureViewModel, value); }
        }

        public OptionsViewModel(ICultureViewModel culture)
        {
            CultureViewModel = culture ?? throw new ArgumentNullException(nameof(culture), $"{nameof(culture)} {Resources.IsRequired}");
        }
    }
}
