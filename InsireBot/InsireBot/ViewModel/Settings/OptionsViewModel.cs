using Maple.Core;
using System.Globalization;
using System.Threading.Tasks;

namespace Maple
{
    public class OptionsViewModel : ObservableObject, IRefreshable
    {
        private ITranslationManager _manager;
        public RangeObservableCollection<CultureInfo> Items { get; private set; }

        private CultureInfo _selectedCulture;
        public CultureInfo SelectedCulture
        {
            get { return _selectedCulture; }
            set { SetValue(ref _selectedCulture, value, Changed: SyncCulture); }
        }

        public OptionsViewModel(ITranslationManager manager)
        {
            _manager = manager;
            Items = new RangeObservableCollection<CultureInfo>(_manager.Languages);
            SelectedCulture = Properties.Settings.Default.StartUpCulture;
        }

        private void SyncCulture()
        {
            _manager.CurrentLanguage = SelectedCulture;
        }

        public Task SaveAsync()
        {
            return _manager.SaveAsync();
        }

        public Task LoadAsync()
        {
            return _manager.LoadAsync();
        }
    }
}
