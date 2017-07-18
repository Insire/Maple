﻿using Maple.Core;
using Maple.Localization.Properties;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maple
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Maple.Core.ObservableObject" />
    /// <seealso cref="Maple.Core.ILoadableViewModel" />
    /// <seealso cref="Maple.Core.ISaveableViewModel" />
    public class UIColorsViewModel : ObservableObject, IUIColorsViewModel
    {
        private readonly ILoggingService _log;
        private readonly IMessenger _messenger;

        private static bool _isDark;
        private static string _accent;
        private static string _swatch;

        private static PaletteHelper _paletteHelper = new PaletteHelper();

        /// <summary>
        /// Gets a value indicating whether this instance is loaded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is loaded; otherwise, <c>false</c>.
        /// </value>
        public bool IsLoaded { get; private set; }

        /// <summary>
        /// Gets the toggle base command.
        /// </summary>
        /// <value>
        /// The toggle base command.
        /// </value>
        public ICommand ToggleBaseCommand { get; private set; }
        /// <summary>
        /// Gets the apply primary command.
        /// </summary>
        /// <value>
        /// The apply primary command.
        /// </value>
        public ICommand ApplyPrimaryCommand { get; private set; }
        /// <summary>
        /// Gets the apply accent command.
        /// </summary>
        /// <value>
        /// The apply accent command.
        /// </value>
        public ICommand ApplyAccentCommand { get; private set; }

        /// <summary>
        /// Gets the refresh command.
        /// </summary>
        /// <value>
        /// The refresh command.
        /// </value>
        public ICommand RefreshCommand => new RelayCommand(Load);
        /// <summary>
        /// Gets the load command.
        /// </summary>
        /// <value>
        /// The load command.
        /// </value>
        public ICommand LoadCommand => new RelayCommand(Load, () => !IsLoaded);
        /// <summary>
        /// Gets the save command.
        /// </summary>
        /// <value>
        /// The save command.
        /// </value>
        public ICommand SaveCommand => new RelayCommand(Save);

        /// <summary>
        /// Gets the swatches.
        /// </summary>
        /// <value>
        /// The swatches.
        /// </value>
        public static IEnumerable<Swatch> Swatches => new SwatchesProvider().Swatches;

        /// <summary>
        /// Initializes a new instance of the <see cref="UIColorsViewModel"/> class.
        /// </summary>
        public UIColorsViewModel(ViewModelServiceContainer container)
        {
            _log = container.Log;
            _messenger = container.Messenger;

            OnPropertyChanged(nameof(Swatches));
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            ToggleBaseCommand = new RelayCommand(() => ApplyBase(this, !_isDark));
            ApplyPrimaryCommand = new RelayCommand<Swatch>(o => ApplyPrimary(this, o));
            ApplyAccentCommand = new RelayCommand<Swatch>(o => ApplyAccent(this, o));
        }

        private static void ApplyBase(IUIColorsViewModel vm, bool isDark = false)
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

        private static void ApplyAccent(IUIColorsViewModel vm, Swatch swatch)
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
        public void Save()
        {
            _log.Info($"{Resources.Saving} {Resources.Themes}");

            Properties.Settings.Default.AccentName = _accent;
            Properties.Settings.Default.SwatchName = _swatch;
            Properties.Settings.Default.UseDarkTheme = _isDark;

            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load()
        {
            _log.Info($"{Resources.Loading} {Resources.Themes}");

            var swatchName = Properties.Settings.Default.SwatchName;
            var swatch = Swatches.FirstOrDefault(p => p.Name == swatchName);

            var accentName = Properties.Settings.Default.AccentName;
            var accent = Swatches.FirstOrDefault(p => p.Name == accentName);

            ApplyPrimary(this, swatch);
            ApplyAccent(this, accent);
            ApplyBase(this, Properties.Settings.Default.UseDarkTheme);

            _messenger.Publish(new LoadedMessage(this, this));
        }

        public Task SaveAsync()
        {
            return Task.Run(() => Save());
        }

        public Task LoadAsync()
        {
            Load();
            return Task.CompletedTask;
        }
    }
}
