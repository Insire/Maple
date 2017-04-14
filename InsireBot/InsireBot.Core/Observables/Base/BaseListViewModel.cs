using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace Maple.Core
{
    /// <summary>
    /// ListViewModel implementation for ObservableObjects unrelated to the DataAccessLayer (DB)
    /// </summary>
    /// <typeparam name="T">a class implementing <see cref="ObservableObject" /></typeparam>
    /// <seealso cref="Maple.Core.ObservableObject" />
    public abstract class BaseListViewModel<T> : ObservableObject where T : INotifyPropertyChanged
    {
        /// <summary>
        /// The items lock
        /// </summary>
        protected object _itemsLock;

        /// <summary>
        /// The selection changed event
        /// </summary>
        public EventHandler SelectionChanged;
        /// <summary>
        /// The selection changing event
        /// </summary>
        public EventHandler SelectionChanging;

        private bool _isBusy;
        /// <summary>
        /// Indicates if there is an operation running.
        /// Modified by adding <see cref="BusyToken" /> to the <see cref="BusyStack" /> property
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is busy; otherwise, <c>false</c>.
        /// </value>
        public bool IsBusy
        {
            get { return _isBusy; }
            private set { SetValue(ref _isBusy, value); }
        }

        private BusyStack _busyStack;
        /// <summary>
        /// Provides IDisposable tokens for running async operations
        /// </summary>
        /// <value>
        /// The busy stack.
        /// </value>
        protected BusyStack BusyStack
        {
            get { return _busyStack; }
            private set { SetValue(ref _busyStack, value); }
        }

        private T _selectedItem;
        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>
        /// The selected item.
        /// </value>
        public virtual T SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (EqualityComparer<T>.Default.Equals(_selectedItem, value))
                    return;

                SelectionChanging?.Raise(this);
                _selectedItem = value;
                SelectionChanged?.Raise(this);

                OnPropertyChanged();
            }
        }

        private RangeObservableCollection<T> _items;
        /// <summary>
        /// Contains all the UI relevant Models and notifies about changes in the collection and inside the Models themself
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public RangeObservableCollection<T> Items
        {
            get { return _items; }
            private set { SetValue(ref _items, value); }
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
            protected set { SetValue(ref _view, value); }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count => Items?.Count ?? 0;
        /// <summary>
        /// Gets the <see cref="T"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="T"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public T this[int index]
        {
            get { return Items[index]; }
        }

        /// <summary>
        /// Gets or sets the remove range command.
        /// </summary>
        /// <value>
        /// The remove range command.
        /// </value>
        public ICommand RemoveRangeCommand { get; protected set; }
        /// <summary>
        /// Gets or sets the remove command.
        /// </summary>
        /// <value>
        /// The remove command.
        /// </value>
        public ICommand RemoveCommand { get; protected set; }
        /// <summary>
        /// Gets or sets the clear command.
        /// </summary>
        /// <value>
        /// The clear command.
        /// </value>
        public ICommand ClearCommand { get; protected set; }
        /// <summary>
        /// Gets or sets the add command.
        /// </summary>
        /// <value>
        /// The add command.
        /// </value>
        public ICommand AddCommand { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseListViewModel{T}"/> class.
        /// </summary>
        public BaseListViewModel()
        {
            InitializeProperties();
            InitializeCommands();

            BindingOperations.EnableCollectionSynchronization(Items, _itemsLock);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseListViewModel{T}"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        public BaseListViewModel(IList<T> items) : this()
        {
            Items.AddRange(items);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseListViewModel{T}"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        public BaseListViewModel(IEnumerable<T> items) : this()
        {
            Items.AddRange(items);
        }

        private void InitializeProperties()
        {
            _itemsLock = new object();

            Items = new RangeObservableCollection<T>();
            Items.CollectionChanged += ItemsCollectionChanged;

            BusyStack = new BusyStack()
            {
                OnChanged = (hasItems) => IsBusy = hasItems
            };

            View = CollectionViewSource.GetDefaultView(Items);

            // initial Notification, so that UI recognizes the value
            OnPropertyChanged(nameof(Count));
        }

        private void InitializeCommands()
        {
            RemoveCommand = new RelayCommand<T>(Remove, CanRemove);
            RemoveRangeCommand = new RelayCommand<IList>(RemoveRange, CanRemoveRange);
            ClearCommand = new RelayCommand(() => Clear(), CanClear);
        }

        private void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Count));
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <exception cref="System.ArgumentNullException">item</exception>
        public virtual void Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            using (BusyStack.GetToken())
                Items.Add(item);
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <exception cref="System.ArgumentNullException">items</exception>
        public virtual void AddRange(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            using (BusyStack.GetToken())
                Items.AddRange(items);
        }

        /// <summary>
        /// Determines whether this instance can add.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can add; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool CanAdd()
        {
            return Items != null;
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public virtual void Remove(T item)
        {
            using (BusyStack.GetToken())
                Items.Remove(item);
        }

        /// <summary>
        /// Removes the range.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <exception cref="System.ArgumentNullException">items</exception>
        public virtual void RemoveRange(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            using (BusyStack.GetToken())
                Items.RemoveRange(items);
        }

        /// <summary>
        /// Removes the range.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <exception cref="System.ArgumentNullException">items</exception>
        public virtual void RemoveRange(IList items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            using (BusyStack.GetToken())
                Items.RemoveRange(items);
        }

        /// <summary>
        /// Determines whether this instance can remove the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if this instance can remove the specified item; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool CanRemove(T item)
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
        protected virtual bool CanRemoveRange(IEnumerable<T> items)
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
        protected virtual bool CanRemoveRange(IList items)
        {
            return items == null ? false : CanRemoveRange(items.Cast<T>());
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public virtual void Clear()
        {
            using (BusyStack.GetToken())
                Items.Clear();
        }

        /// <summary>
        /// Determines whether this instance can clear.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can clear; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool CanClear()
        {
            return Items?.Any() == true;
        }
    }
}
