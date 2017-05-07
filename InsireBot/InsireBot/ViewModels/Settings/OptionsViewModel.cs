using Maple.Core;
using System;

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

        private IUIColorsViewModel _uiColorsViewModel;
        public IUIColorsViewModel UIColorsViewModel
        {
            get { return _uiColorsViewModel; }
            set { SetValue(ref _uiColorsViewModel, value); }
        }

        public OptionsViewModel(IUIColorsViewModel colors, ICultureViewModel culture)
        {
            UIColorsViewModel = colors ?? throw new ArgumentNullException(nameof(colors));
            CultureViewModel = culture ?? throw new ArgumentNullException(nameof(culture));
        }
    }
}
