using MvvmScarletToolkit;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace InsireBot
{
    public class OptionsViewModel : ObservableObject
    {
        private ITranslationManager _manager;
        public ICollectionView Languages { get; private set; }

        public OptionsViewModel(ITranslationManager manager)
        {
            _manager = manager;
            Languages = CollectionViewSource.GetDefaultView(_manager.Languages);
            Languages.CurrentChanged += (s, e) => _manager.CurrentLanguage = (CultureInfo)Languages.CurrentItem;
        }
    }
}
