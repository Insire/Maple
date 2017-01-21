using MvvmScarletToolkit;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace InsireBot
{
    public class OptionsViewModel : ObservableObject
    {
        private ITranslationManager _manager;
        public ICollectionView Items { get; private set; }

        public OptionsViewModel(ITranslationManager manager)
        {
            _manager = manager;
            Items = CollectionViewSource.GetDefaultView(_manager.Languages);
            Items.CurrentChanged += (s, e) => _manager.CurrentLanguage = (CultureInfo)Items.CurrentItem;
        }
    }
}
