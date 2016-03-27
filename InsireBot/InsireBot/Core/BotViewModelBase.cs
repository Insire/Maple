using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace InsireBot
{
    public class BotViewModelBase<T> : ViewModelBase
    {
        protected readonly IDataService _dataService;

        private object _ItemsLock;
        private object _FilteredItemsLock;

        private int _SelectedIndex = -1;
        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                if (value != _SelectedIndex)
                {
                    _SelectedIndex = value;

                    if (_SelectedIndex >= Items.Count)
                        SelectedIndex = 0;

                    if (_SelectedIndex < 0)
                        SelectedIndex = Items.Count - 1;

                    RaisePropertyChanged(nameof(SelectedIndex));
                    RaisePropertyChanged(nameof(SelectedItem));
                }
            }
        }

        private RangeObservableCollection<T> _Items;
        public RangeObservableCollection<T> Items
        {
            get { return _Items; }
            set
            {
                _Items = value;
                RaisePropertyChanged(nameof(Items));
            }
        }

        public ICommand RemoveCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }

        public int Count()
        {
            return Items.Count;
        }

        public T SelectedItem
        {
            get { return Items[SelectedIndex]; }
        }

        public T this[int index]
        {
            get
            {
                return Items[index];
            }
        }

        private ICollectionView _filteredUsertasksView;
        public ICollectionView FilteredItemsView
        {
            get { return _filteredUsertasksView; }
            private set
            {
                _filteredUsertasksView = value;
                RaisePropertyChanged(nameof(FilteredItemsView));
            }
        }

        public BotViewModelBase(IDataService dataService)
        {
            _dataService = dataService;

            _ItemsLock = new object();
            _FilteredItemsLock = new object();

            Items = new RangeObservableCollection<T>();
            FilteredItemsView = CollectionViewSource.GetDefaultView(Items);

            BindingOperations.EnableCollectionSynchronization(Items, _ItemsLock);

            RemoveCommand = new RelayCommand(() => Items.Remove(Items[SelectedIndex]), CanRemove);
            ClearCommand = new RelayCommand(() => Items.Clear(), CanClear);
        }

        internal bool CanRemove()
        {
            return IsItemSelected() && Items.Contains(SelectedItem);
        }

        private bool CanClear()
        {
            return Items?.Count > 0;
        }

        private bool IsItemSelected()
        {
            return -1 < SelectedIndex && SelectedIndex < Items.Count;
        }
    }
}
