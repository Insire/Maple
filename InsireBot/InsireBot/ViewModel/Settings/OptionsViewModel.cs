using Maple.Core;
using System.Globalization;
using System.Windows.Input;

namespace Maple
{
    public class OptionsViewModel : ObservableObject, ILoadableViewModel, ISaveableViewModel
    {
        private readonly ITranslationService _manager;

        private RangeObservableCollection<CultureInfo> _items;
        public RangeObservableCollection<CultureInfo> Items
        {
            get { return _items; }
            set { SetValue(ref _items, value); }
        }

        private CultureInfo _selectedCulture;
        public CultureInfo SelectedCulture
        {
            get { return _selectedCulture; }
            set { SetValue(ref _selectedCulture, value, OnChanged: SyncCulture); }
        }

        public bool IsLoaded { get; private set; }

        public ICommand RefreshCommand => new RelayCommand(Load);
        public ICommand LoadCommand => new RelayCommand(Load, () => !IsLoaded);
        public ICommand SaveCommand => new RelayCommand(Save);

        public OptionsViewModel(ITranslationService manager)
        {
            _manager = manager;
            Items = new RangeObservableCollection<CultureInfo>(_manager.Languages);
        }

        private void SyncCulture()
        {
            _manager.CurrentLanguage = SelectedCulture;
        }

        public void Save()
        {
            _manager.Save();
        }

        public void Load()
        {
            _manager.Load();
            SelectedCulture = Properties.Settings.Default.StartUpCulture;
            IsLoaded = true;
        }
    }
}
