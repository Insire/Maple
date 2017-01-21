using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using MvvmScarletToolkit;
using System.Collections.Generic;
using System.Windows.Input;
using System.Linq;

namespace InsireBot
{
    public class UIColorsViewModel : ObservableObject
    {
        public static void ApplyColorsFromSettings()
        {
            var swatchName = Properties.Settings.Default.SwatchName;
            var swatch = Swatches.FirstOrDefault(p => p.Name == swatchName);

            var accentName = Properties.Settings.Default.AccentName;
            var accent = Swatches.FirstOrDefault(p => p.Name == accentName);

            ApplyPrimary(swatch);
            ApplyAccent(accent);
            ApplyBase(Properties.Settings.Default.UseDarkTheme);
        }

        private static PaletteHelper _paletteHelper = new PaletteHelper();

        public ICommand ToggleBaseCommand { get; private set; }
        public ICommand ApplyPrimaryCommand { get; private set; }
        public ICommand ApplyAccentCommand { get; private set; }

        public static IEnumerable<Swatch> Swatches => new SwatchesProvider().Swatches;

        public UIColorsViewModel()
        {
            OnPropertyChanged(nameof(Swatches));
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            ToggleBaseCommand = new RelayCommand<bool>(o => ApplyBase(o));
            ApplyPrimaryCommand = new RelayCommand<Swatch>(o => ApplyPrimary(o));
            ApplyAccentCommand = new RelayCommand<Swatch>(o => ApplyAccent(o));
        }

        private static void ApplyBase(bool isDark = false)
        {
            _paletteHelper.SetLightDark(isDark);
            Properties.Settings.Default.UseDarkTheme = isDark;
            Properties.Settings.Default.Save();
        }

        private static void ApplyPrimary(Swatch swatch)
        {
            if (swatch == null)
                return;

            _paletteHelper.ReplacePrimaryColor(swatch);
            Properties.Settings.Default.SwatchName = swatch.Name;
            Properties.Settings.Default.Save();
        }

        private static void ApplyAccent(Swatch swatch)
        {
            if (swatch == null)
                return;

            _paletteHelper.ReplaceAccentColor(swatch);
            Properties.Settings.Default.AccentName = swatch.Name;
            Properties.Settings.Default.Save();
        }
    }
}
