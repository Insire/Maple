using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

using GalaSoft.MvvmLight.CommandWpf;

namespace InsireBotCore
{
    /// <summary>
    /// handles storing temporary data in the application
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataViewModelBase<T> : DefaultViewModelBase<T> where T : IIndex, IIdentifier, IIsSelected
    {
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

        public bool AreAllItemsSelected
        {
            get { return Items.All(p => p.IsSelected); }
            set
            {
                Items.ToList().ForEach(p => p.IsSelected = value);
                RaisePropertyChanged(nameof(AreAllItemsSelected));
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
            get { return Items[index]; }
        }

        public DataViewModelBase() : base()
        {
            FilteredItemsView = CollectionViewSource.GetDefaultView(Items);

            RemoveCommand = new RelayCommand(() => SelectedItems.ToList().ForEach(p => Items.Remove(p)), CanRemove);
            ClearCommand = new RelayCommand(() => Items.Clear(), CanClear);
        }

        /// <summary>
        /// Can an Item be added to the Items Collection
        /// </summary>
        /// <returns></returns>
        protected virtual bool CanAdd()
        {
            return Items != null;
        }

        public bool CanRemove()
        {
            return CanClear() && AreItemsSelected();
        }

        protected bool CanClear()
        {
            return Items?.Any() == true;
        }

        protected bool AreItemsSelected()
        {
            return Items.Any(p => p.IsSelected);
        }

        public virtual void Add(T item)
        {
            if (CanAdd())
                Items.Add(item);
        }

        public virtual void AddRange(IEnumerable<T> items)
        {
            if (CanAdd())
                Items.AddRange(items);
        }

        public void Clear()
        {
            Items?.Clear();
        }
    }
}
