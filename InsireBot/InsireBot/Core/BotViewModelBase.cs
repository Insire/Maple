using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace InsireBot
{
    public class BotViewModelBase<T> : ViewModelBase where T : IIsSelected
    {
        protected readonly IDataService _dataService;

        private object _itemsLock;
        private object _FilteredItemsLock;

        private int _selectedIndex = -1;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (value != _selectedIndex)
                {
                    _selectedIndex = value;

                    if (_selectedIndex >= Items.Count)
                        SelectedIndex = 0;

                    if (_selectedIndex < 0)
                        SelectedIndex = Items.Count - 1;

                    RaisePropertyChanged(nameof(SelectedIndex));
                    RaisePropertyChanged(nameof(SelectedItem));
                }
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
        public ICommand NewCommand { get; protected set; }
        public ICommand RemoveCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }

        public int Count()
        {
            return Items.Count;
        }

        public T SelectedItem
        {
            get { return SelectedItems.FirstOrDefault(); }
        }

        public IEnumerable<T> SelectedItems
        {
            get { return Items.Where(p => p.IsSelected); }
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

            _itemsLock = new object();
            _FilteredItemsLock = new object();

            Items = new RangeObservableCollection<T>();
            FilteredItemsView = CollectionViewSource.GetDefaultView(Items);

            BindingOperations.EnableCollectionSynchronization(Items, _itemsLock);

            RemoveCommand = new RelayCommand(() => SelectedItems.ToList().ForEach(p=>Items.Remove(p)), CanRemove);
            ClearCommand = new RelayCommand(() => Items.Clear(), CanClear);
        }

        public bool CanRemove()
        {
            return AreItemsSelected();
        }

        protected bool CanClear()
        {
            return Items?.Count > 0;
        }

        protected bool AreItemsSelected()
        {
            return Items.Any(p => p.IsSelected);
        }
    }
}
