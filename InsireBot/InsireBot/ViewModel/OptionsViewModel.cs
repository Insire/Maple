using MvvmScarletToolkit;
using System.Globalization;

namespace InsireBot
{
    public class OptionsViewModel : ObservableObject
    {
        private ITranslationManager _manager;
        public RangeObservableCollection<CultureInfo> Items { get; private set; }

        private CultureInfo _selectedCulture;
        public CultureInfo SelectedCulture
        {
            get { return _selectedCulture; }
            set { SetValue(ref _selectedCulture, value, SyncCulture); }
        }

        public OptionsViewModel(ITranslationManager manager)
        {
            _manager = manager;
            Items = new RangeObservableCollection<CultureInfo>(_manager.Languages);
            SelectedCulture = Properties.Settings.Default.StartUpCulture;
        }

        private void SyncCulture()
        {
            Properties.Settings.Default.StartUpCulture = SelectedCulture;
            Properties.Settings.Default.Save();

            _manager.CurrentLanguage = SelectedCulture;
        }
    }
}
