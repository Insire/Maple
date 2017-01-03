using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using MvvmScarletToolkit;
using System.Collections.Generic;
using System.Windows.Input;

namespace InsireBot
{
    public class UIColorsViewModel : ObservableObject
    {
        private PaletteHelper _paletteHelper;

        public ICommand ToggleBaseCommand { get; private set; }
        public ICommand ApplyPrimaryCommand { get; private set; }
        public ICommand ApplyAccentCommand { get; private set; }

        public IEnumerable<Swatch> Swatches { get; }

        public UIColorsViewModel()
        {
            Swatches = new SwatchesProvider().Swatches;

            _paletteHelper = new PaletteHelper();

            OnPropertyChanged(nameof(Swatches));
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            ToggleBaseCommand = new RelayCommand<bool>(o => ApplyBase(o));
            ApplyPrimaryCommand = new RelayCommand<Swatch>(o => ApplyPrimary(o));
            ApplyAccentCommand = new RelayCommand<Swatch>(o => ApplyAccent(o));
        }

        private void ApplyBase(bool isDark = false)
        {
            _paletteHelper.SetLightDark(isDark);
        }

        private void ApplyPrimary(Swatch swatch)
        {
            _paletteHelper.ReplacePrimaryColor(swatch);
        }

        private void ApplyAccent(Swatch swatch)
        {
            _paletteHelper.ReplaceAccentColor(swatch);
        }
    }
}
