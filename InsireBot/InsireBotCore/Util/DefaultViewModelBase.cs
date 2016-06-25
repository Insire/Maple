using System.Windows.Data;
using GalaSoft.MvvmLight;

namespace InsireBotCore
{
    public class DefaultViewModelBase<T> : ViewModelBase
    {
        protected object _itemsLock;

        private RangeObservableCollection<T> _items;
        public RangeObservableCollection<T> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                RaisePropertyChanged(nameof(Items));
            }
        }

        public DefaultViewModelBase()
        {
            _itemsLock = new object();

            Items = new RangeObservableCollection<T>();

            BindingOperations.EnableCollectionSynchronization(Items, _itemsLock);
        }
    }
}
