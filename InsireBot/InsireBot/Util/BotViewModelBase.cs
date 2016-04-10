using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

using InsireBotCore;

namespace InsireBot
{
    public class BotViewModelBase<T> : ViewModelBase where T : IIsSelected, IIndex
    {
        protected readonly IDataService _dataService;

        private object _itemsLock;

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

        /// <summary>
        /// evaluates based on the items which have the property IsSelected set to true
        /// </summary>
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

            Items = new RangeObservableCollection<T>();
            FilteredItemsView = CollectionViewSource.GetDefaultView(Items);

            BindingOperations.EnableCollectionSynchronization(Items, _itemsLock);

            RemoveCommand = new RelayCommand(() => SelectedItems.ToList().ForEach(p => Items.Remove(p)), CanRemove);
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
