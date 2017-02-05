using Maple.Core;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Maple
{
    public class UIColorsViewModel : ObservableObject, ISaveable
    {
        private static bool _isDark;
        private static string _accent;
        private static string _swatch;

        public void ApplyColorsFromSettings()
        {
            var swatchName = Properties.Settings.Default.SwatchName;
            var swatch = Swatches.FirstOrDefault(p => p.Name == swatchName);

            var accentName = Properties.Settings.Default.AccentName;
            var accent = Swatches.FirstOrDefault(p => p.Name == accentName);

            ApplyPrimary(this,swatch);
            ApplyAccent(this, accent);
            ApplyBase(this, Properties.Settings.Default.UseDarkTheme);
        }

        private static PaletteHelper _paletteHelper = new PaletteHelper();

        public ICommand ToggleBaseCommand { get; private set; }
        public ICommand ApplyPrimaryCommand { get; private set; }
        public ICommand ApplyAccentCommand { get; private set; }

        public EventHandler<UiPrimaryColorEventArgs> PrimaryColorChanged;

        public static IEnumerable<Swatch> Swatches => new SwatchesProvider().Swatches;

        public UIColorsViewModel()
        {
            OnPropertyChanged(nameof(Swatches));
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            ToggleBaseCommand = new RelayCommand<bool>(o => ApplyBase(this, o));
            ApplyPrimaryCommand = new RelayCommand<Swatch>(o => ApplyPrimary(this, o));
            ApplyAccentCommand = new RelayCommand<Swatch>(o => ApplyAccent(this, o));
        }

        private static void ApplyBase(UIColorsViewModel vm, bool isDark = false)
        {
            _paletteHelper.SetLightDark(isDark);
            _isDark = isDark;
        }

        private static void ApplyPrimary(UIColorsViewModel vm, Swatch swatch)
        {
            if (swatch == null)
                return;

            var oldPalette = _paletteHelper.QueryPalette();
            _paletteHelper.ReplacePrimaryColor(swatch);
            var newPalette = _paletteHelper.QueryPalette();

            if (newPalette.PrimarySwatch.Name != oldPalette.PrimarySwatch.Name)
                vm.PrimaryColorChanged?.Invoke(vm, new UiPrimaryColorEventArgs(newPalette.PrimarySwatch.ExemplarHue.Color));

            _swatch = swatch.Name;
        }

        private static void ApplyAccent(UIColorsViewModel vm, Swatch swatch)
        {
            if (swatch == null)
                return;

            _paletteHelper.ReplaceAccentColor(swatch);
            _accent = swatch.Name;
        }

        public void Save()
        {
            Properties.Settings.Default.AccentName = _accent;
            Properties.Settings.Default.SwatchName = _swatch;
            Properties.Settings.Default.UseDarkTheme = _isDark;

            Properties.Settings.Default.Save();
        }
    }
}
