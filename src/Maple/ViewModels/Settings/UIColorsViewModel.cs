using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Maple.Core;
using Maple.Domain;
using Maple.Localization.Properties;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Logging;
using MvvmScarletToolkit.Commands;

namespace Maple
{
    public sealed class UIColorsViewModel : MapleBusinessViewModelBase, IUIColorsViewModel
    {
        private static bool _isDark;
        private static string _accent;
        private static string _swatch;

        private readonly PaletteHelper _paletteHelper;
        private readonly SwatchesProvider _swatchesProvider;
        private readonly ILogger _log;

        public ICommand ToggleBaseCommand { get; }

        public ICommand ApplyPrimaryCommand { get; }
        public ICommand ApplyAccentCommand { get; }

        public ICommand SaveCommand { get; }

        public IEnumerable<Swatch> Swatches => _swatchesProvider.Swatches;

        public UIColorsViewModel(IMapleCommandBuilder commandBuilder, SwatchesProvider swatchesProvider, PaletteHelper paletteHelper)
            : base(commandBuilder)
        {
            _swatchesProvider = swatchesProvider ?? throw new ArgumentNullException(nameof(swatchesProvider));
            _paletteHelper = paletteHelper ?? throw new ArgumentNullException(nameof(paletteHelper));
            _log = LogFactory.CreateLogger<UIColorsViewModel>();

            OnPropertyChanged(nameof(Swatches));
            ToggleBaseCommand = new RelayCommand(() => ApplyBase(!_isDark));
            ApplyPrimaryCommand = new RelayCommand<Swatch>(o => ApplyPrimary(this, o));
            ApplyAccentCommand = new RelayCommand<Swatch>(o => ApplyAccent(o));
        }

        private void ApplyBase(bool isDark = false)
        {
            _paletteHelper.SetLightDark(isDark);
            _isDark = isDark;
        }

        private void ApplyPrimary(IUIColorsViewModel vm, Swatch swatch)
        {
            if (swatch is null)
                return;

            var oldPalette = _paletteHelper.QueryPalette();
            _paletteHelper.ReplacePrimaryColor(swatch);
            var newPalette = _paletteHelper.QueryPalette();

            if (newPalette.PrimarySwatch.Name != oldPalette.PrimarySwatch.Name)
                vm.OnPrimaryColorChanged(new UiPrimaryColorChangedMessage(vm, newPalette.PrimarySwatch.ExemplarHue.Color));

            _swatch = swatch.Name;
        }

        private void ApplyAccent(Swatch swatch)
        {
            if (swatch == null)
                return;

            _paletteHelper.ReplaceAccentColor(swatch);
            _accent = swatch.Name;
        }

        public void OnPrimaryColorChanged(UiPrimaryColorChangedMessage args)
        {
            Messenger.Publish(args);
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public Task Save()
        {
            _log.LogInformation($"{Resources.Saving} {Resources.Themes}");

            Properties.Settings.Default.AccentName = _accent;
            Properties.Settings.Default.SwatchName = _swatch;
            Properties.Settings.Default.UseDarkTheme = _isDark;

            Properties.Settings.Default.Save();

            return Task.CompletedTask;
        }

        protected override Task UnloadInternal(CancellationToken token)
        {
            throw new System.NotImplementedException();
        }

        protected override Task RefreshInternal(CancellationToken token)
        {
            _log.LogInformation($"{Resources.Loading} {Resources.Themes}");

            var swatchName = Properties.Settings.Default.SwatchName;
            var swatch = Swatches.FirstOrDefault(p => p.Name == swatchName);

            var accentName = Properties.Settings.Default.AccentName;
            var accent = Swatches.FirstOrDefault(p => p.Name == accentName);

            ApplyPrimary(this, swatch);
            ApplyAccent(accent);
            ApplyBase(Properties.Settings.Default.UseDarkTheme);

            Messenger.Publish(new LoadedMessage(this, this));

            return Task.CompletedTask;
        }
    }
}
