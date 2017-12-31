using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Maple.Domain;

namespace Maple.Core
{
    /// <summary>
    /// ListViewModel implementation for ObservableObjects unrelated to the DataAccessLayer (DB)
    /// </summary>
    /// <typeparam name="TViewModel">a class implementing <see cref="ObservableObject" /></typeparam>
    /// <seealso cref="Maple.Core.ObservableObject" />
    public abstract class BaseListViewModel<TViewModel> : ViewModel
        where TViewModel : INotifyPropertyChanged
    {
        protected readonly object _itemsLock;

        /// <summary>
        /// Indicates whether the LoadCommand/ the Load Method has been executed yet
        /// </summary>
        public bool IsLoaded { get; protected set; }

        private TViewModel _selectedItem;
        public virtual TViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (EqualityComparer<TViewModel>.Default.Equals(_selectedItem, value))
                    return;

                Messenger.Publish(new ViewModelSelectionChangingMessage<TViewModel>(Items, _selectedItem));
                _selectedItem = value;
                Messenger.Publish(new ViewModelSelectionChangedMessage<TViewModel>(Items, _selectedItem));

                OnPropertyChanged();
            }
        }

        private IRangeObservableCollection<TViewModel> _items;
        /// <summary>
        /// Contains all the UI relevant Models and notifies about changes in the collection and inside the Models themself
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public IReadOnlyCollection<TViewModel> Items
        {
            get { return (IReadOnlyCollection<TViewModel>)_items; }
            private set { SetValue(ref _items, (IRangeObservableCollection<TViewModel>)value); }
        }

        private ICollectionView _view;
        /// <summary>
        /// For grouping, sorting and filtering
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public ICollectionView View
        {
            get { return _view; }
            private set { SetValue(ref _view, value); }
        }

        public int Count => Items?.Count ?? 0;
        /// <summary>
        /// Gets the <see cref="TViewModel"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="TViewModel"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public TViewModel this[int index]
        {
            get { return _items[index]; }
        }

        public ICommand RemoveRangeCommand { get; private set; }
        public ICommand RemoveCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }
        public ICommand AddCommand { get; protected set; }

        protected BaseListViewModel(IMessenger messenger)
            : base(messenger)
        {
            _itemsLock = new object();


            Items = new RangeObservableCollection<TViewModel>();
            _items.CollectionChanged += ItemsCollectionChanged;

            View = CollectionViewSource.GetDefaultView(Items);

            // initial Notification, so that UI recognizes the value
            OnPropertyChanged(nameof(Count));

            InitializeCommands();

            BindingOperations.EnableCollectionSynchronization(Items, _itemsLock);
        }

        protected BaseListViewModel(IList<TViewModel> items, IMessenger messenger)
            : this(messenger)
        {
            AddRange(items);
        }

        protected BaseListViewModel(IEnumerable<TViewModel> items, IMessenger messenger)
            : this(messenger)
        {
            AddRange(items);
        }

        private void InitializeCommands()
        {
            RemoveCommand = new RelayCommand<TViewModel>(Remove, CanRemove);
            RemoveRangeCommand = new RelayCommand<IList>(RemoveRange, CanRemoveRange);
            ClearCommand = new RelayCommand(() => Clear(), CanClear);
        }

        private void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Count));
        }

        protected virtual void OnLoaded()
        {
            IsLoaded = true;
            Messenger.Publish(new LoadedMessage(this, this));
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <exception cref="System.ArgumentNullException">item</exception>
        public virtual void Add(TViewModel item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            using (BusyStack.GetToken())
                _items.Add(item);
        }

        /// <param name="items">The items.</param>
        /// <exception cref="System.ArgumentNullException">items</exception>
        public virtual void AddRange(IEnumerable<TViewModel> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            using (BusyStack.GetToken())
                _items.AddRange(items);
        }

        protected virtual bool CanAdd(TViewModel item)
        {
            return Items != null && item != null;
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public virtual void Remove(TViewModel item)
        {
            using (BusyStack.GetToken())
                _items.Remove(item);
        }

        /// <param name="items">The items.</param>
        /// <exception cref="System.ArgumentNullException">items</exception>
        public virtual void RemoveRange(IEnumerable<TViewModel> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            using (BusyStack.GetToken())
                _items.RemoveRange(items);
        }

        /// <param name="items">The items.</param>
        /// <exception cref="System.ArgumentNullException">items</exception>
        public virtual void RemoveRange(IList items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            using (BusyStack.GetToken())
                _items.RemoveRange(items.Cast<TViewModel>());
        }

        /// <summary>
        /// Determines whether this instance can remove the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if this instance can remove the specified item; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool CanRemove(TViewModel item)
        {
            return CanClear() && item != null && Items.Contains(item);
        }

        /// <summary>
        /// Checks if any of the submitted items can be removed
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>
        ///   <c>true</c> if this instance [can remove range] the specified items; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool CanRemoveRange(IEnumerable<TViewModel> items)
        {
            return CanClear() && items != null && items.Any(p => Items.Contains(p));
        }

        /// <summary>
        /// Determines whether this instance [can remove range] the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>
        ///   <c>true</c> if this instance [can remove range] the specified items; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool CanRemoveRange(IList items)
        {
            return items == null ? false : CanRemoveRange(items.Cast<TViewModel>());
        }

        public virtual void Clear()
        {
            SelectedItem = default(TViewModel);

            using (BusyStack.GetToken())
                _items.Clear();
        }

        public virtual bool CanClear()
        {
            return Items?.Count > 0 && !IsBusy;
        }
    }
}
