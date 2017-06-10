using Maple.Core;
using Maple.Localization.Properties;
using System;
using System.Globalization;
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
    public class CultureViewModel : ObservableObject, ICultureViewModel
    {
        private readonly ILocalizationService _manager;
        private readonly IMapleLog _log;

        public event LoadedEventHandler Loaded;

        private RangeObservableCollection<CultureInfo> _items;
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public RangeObservableCollection<CultureInfo> Items
        {
            get { return _items; }
            set { SetValue(ref _items, value); }
        }

        private CultureInfo _selectedCulture;
        /// <summary>
        /// Gets or sets the selected culture.
        /// </summary>
        /// <value>
        /// The selected culture.
        /// </value>
        public CultureInfo SelectedCulture
        {
            get { return _selectedCulture; }
            set { SetValue(ref _selectedCulture, value, OnChanged: SyncCulture); }
        }

        public bool IsLoaded { get; private set; }

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
        /// Initializes a new instance of the <see cref="CultureViewModel"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public CultureViewModel(ILocalizationService manager, IMapleLog log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _manager = manager ?? throw new ArgumentNullException(nameof(log));

            Items = new RangeObservableCollection<CultureInfo>(_manager.Languages);
        }

        private void SyncCulture()
        {
            _manager.CurrentLanguage = SelectedCulture;
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            _log.Info($"{Resources.Saving} {Resources.Options}");
            _manager.Save();
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load()
        {
            _log.Info($"{Resources.Loading} {Resources.Options}");
            _manager.Load();
            SelectedCulture = Core.Properties.Settings.Default.StartUpCulture;
            IsLoaded = true;
            Loaded?.Invoke(this, new LoadedEventEventArgs());
        }

        public async Task SaveAsync()
        {
            _log.Info($"{Resources.Saving} {Resources.Options}");
            await _manager.SaveAsync();
        }

        public async Task LoadAsync()
        {
            _log.Info($"{Resources.Loading} {Resources.Options}");
            await _manager.LoadAsync();
            SelectedCulture = Core.Properties.Settings.Default.StartUpCulture;
            IsLoaded = true;
            Loaded?.Invoke(this, new LoadedEventEventArgs());
        }
    }
}
