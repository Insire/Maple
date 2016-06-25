using System.Windows.Data;
using GalaSoft.MvvmLight;

namespace InsireBotCore
{
    /// <summary>
    /// the base of all my listbased viewmodels
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DefaultViewModelBase<T> : ViewModelBase
    {
        protected object _itemsLock;

        private BusyStack _busyStack;
        public BusyStack BusyStack
        {
            get { return _busyStack; }
            set
            {
                _busyStack = value;
                RaisePropertyChanged(nameof(BusyStack));
            }
        }

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
            BusyStack = new BusyStack();

            BindingOperations.EnableCollectionSynchronization(Items, _itemsLock);
        }
    }
}
