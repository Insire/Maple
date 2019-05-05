using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Maple.Core;
using Maple.Domain;
using Maple.Localization.Properties;

using MaterialDesignColors;

using MaterialDesignThemes.Wpf;

namespace Maple
{
    public sealed class UIColorsViewModel : ObservableObject, IUIColorsViewModel
    {
        private readonly ILoggingService _log;
        private readonly IMessenger _messenger;

        private static bool _isDark;
        private static string _accent;
        private static string _swatch;

        private static PaletteHelper _paletteHelper = new PaletteHelper();

        private bool _isLoaded;
        public bool IsLoaded
        {
            get { return _isLoaded; }
            private set { SetValue(ref _isLoaded, value); }
        }

        private ICommand _toggleBaseCommand;
        public ICommand ToggleBaseCommand
        {
            get { return _toggleBaseCommand; }
            private set { SetValue(ref _toggleBaseCommand, value); }
        }

        private ICommand _applyPrimaryCommand;
        public ICommand ApplyPrimaryCommand
        {
            get { return _applyPrimaryCommand; }
            private set { SetValue(ref _applyPrimaryCommand, value); }
        }

        private ICommand _applyAccentCommand;
        public ICommand ApplyAccentCommand
        {
            get { return _applyAccentCommand; }
            private set { SetValue(ref _applyAccentCommand, value); }
        }

        public ICommand RefreshCommand => AsyncCommand.Create(Load);
        public ICommand LoadCommand => AsyncCommand.Create(Load, () => !IsLoaded);
        public ICommand SaveCommand => AsyncCommand.Create(Save);

        public static IEnumerable<Swatch> Swatches => new SwatchesProvider().Swatches;

        public UIColorsViewModel(ViewModelServiceContainer container)
        {
            _log = container.Log;
            _messenger = container.Messenger;

            OnPropertyChanged(nameof(Swatches));
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            ToggleBaseCommand = new RelayCommand(() => ApplyBase(!_isDark));
            ApplyPrimaryCommand = new RelayCommand<Swatch>(o => ApplyPrimary(this, o));
            ApplyAccentCommand = new RelayCommand<Swatch>(o => ApplyAccent(o));
        }

        private static void ApplyBase(bool isDark = false)
        {
            _paletteHelper.SetLightDark(isDark);
            _isDark = isDark;
        }

        private static void ApplyPrimary(IUIColorsViewModel vm, Swatch swatch)
        {
            if (swatch == null)
                return;

            var oldPalette = _paletteHelper.QueryPalette();
            _paletteHelper.ReplacePrimaryColor(swatch);
            var newPalette = _paletteHelper.QueryPalette();

            if (newPalette.PrimarySwatch.Name != oldPalette.PrimarySwatch.Name)
                vm.OnPrimaryColorChanged(new UiPrimaryColorChangedMessage(vm, newPalette.PrimarySwatch.ExemplarHue.Color));

            _swatch = swatch.Name;
        }

        private static void ApplyAccent(Swatch swatch)
        {
            if (swatch == null)
                return;

            _paletteHelper.ReplaceAccentColor(swatch);
            _accent = swatch.Name;
        }

        public void OnPrimaryColorChanged(UiPrimaryColorChangedMessage args)
        {
            _messenger.Publish(args);
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public Task Save()
        {
            _log.Info($"{Resources.Saving} {Resources.Themes}");

            Properties.Settings.Default.AccentName = _accent;
            Properties.Settings.Default.SwatchName = _swatch;
            Properties.Settings.Default.UseDarkTheme = _isDark;

            Properties.Settings.Default.Save();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public Task Load()
        {
            _log.Info($"{Resources.Loading} {Resources.Themes}");

            var swatchName = Properties.Settings.Default.SwatchName;
            var swatch = Swatches.FirstOrDefault(p => p.Name == swatchName);

            var accentName = Properties.Settings.Default.AccentName;
            var accent = Swatches.FirstOrDefault(p => p.Name == accentName);

            ApplyPrimary(this, swatch);
            ApplyAccent(accent);
            ApplyBase(Properties.Settings.Default.UseDarkTheme);

            _messenger.Publish(new LoadedMessage(this, this));

            return Task.CompletedTask;
        }
    }
}
