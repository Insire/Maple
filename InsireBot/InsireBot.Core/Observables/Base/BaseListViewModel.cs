using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Data;
using System.Windows.Input;

namespace Maple.Core
{
    /// <summary>
    /// ListViewModel implementation for ObservableObjects unrelated to the DataAccessLayer (DB)
    /// </summary>
    /// <typeparam name="TViewModel">a class implementing <see cref="ObservableObject" /></typeparam>
    /// <seealso cref="Maple.Core.ObservableObject" />
    public abstract class BaseListViewModel<TViewModel> : ObservableObject, INotifyDataErrorInfo
        where TViewModel : INotifyPropertyChanged
    {
        private readonly Dictionary<string, List<string>> _errors;
        private readonly Dictionary<string, (List<BaseValidationRule> Items, object Value)> _rules;

        /// <summary>
        /// The items lock
        /// </summary>
        protected readonly object _itemsLock;
        protected readonly BusyStack _busyStack;

        /// <summary>
        /// The selection changed event
        /// </summary>
        public EventHandler SelectionChanged;
        /// <summary>
        /// The selection changing event
        /// </summary>
        public EventHandler SelectionChanging;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => _errors.Count>0;

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

        private TViewModel _selectedItem;
        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>
        /// The selected item.
        /// </value>
        public virtual TViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (EqualityComparer<TViewModel>.Default.Equals(_selectedItem, value))
                    return;

                SelectionChanging?.Raise(this);
                _selectedItem = value;
                SelectionChanged?.Raise(this);

                OnPropertyChanged();
            }
        }

        private RangeObservableCollection<TViewModel> _items;
        /// <summary>
        /// Contains all the UI relevant Models and notifies about changes in the collection and inside the Models themself
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public RangeObservableCollection<TViewModel> Items
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
            private set { SetValue(ref _view, value); }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
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
            get { return Items[index]; }
        }

        /// <summary>
        /// Gets or sets the remove range command.
        /// </summary>
        /// <value>
        /// The remove range command.
        /// </value>
        public ICommand RemoveRangeCommand { get; private set; }
        /// <summary>
        /// Gets or sets the remove command.
        /// </summary>
        /// <value>
        /// The remove command.
        /// </value>
        public ICommand RemoveCommand { get; private set; }
        /// <summary>
        /// Gets or sets the clear command.
        /// </summary>
        /// <value>
        /// The clear command.
        /// </value>
        public ICommand ClearCommand { get; private set; }
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
            _itemsLock = new object();
            _busyStack = new BusyStack();
            _busyStack.OnChanged = (hasItems) => IsBusy = hasItems;
            _errors = new Dictionary<string, List<string>>();
            _rules = new Dictionary<string, (List<BaseValidationRule> Items, object Value)>();

            Items = new RangeObservableCollection<TViewModel>();
            Items.CollectionChanged += ItemsCollectionChanged;

            View = CollectionViewSource.GetDefaultView(Items);

            // initial Notification, so that UI recognizes the value
            OnPropertyChanged(nameof(Count));

            InitializeCommands();

            BindingOperations.EnableCollectionSynchronization(Items, _itemsLock);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseListViewModel{T}"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        public BaseListViewModel(IList<TViewModel> items) : this()
        {
            Items.AddRange(items);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseListViewModel{T}"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        public BaseListViewModel(IEnumerable<TViewModel> items) : this()
        {
            Items.AddRange(items);
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

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <exception cref="System.ArgumentNullException">item</exception>
        public virtual void Add(TViewModel item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            using (_busyStack.GetToken())
                Items.Add(item);
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <exception cref="System.ArgumentNullException">items</exception>
        public virtual void AddRange(IEnumerable<TViewModel> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            using (_busyStack.GetToken())
                Items.AddRange(items);
        }

        /// <summary>
        /// Determines whether this instance can add.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can add; otherwise, <c>false</c>.
        /// </returns>
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
            using (_busyStack.GetToken())
                Items.Remove(item);
        }

        /// <summary>
        /// Removes the range.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <exception cref="System.ArgumentNullException">items</exception>
        public virtual void RemoveRange(IEnumerable<TViewModel> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            using (_busyStack.GetToken())
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

            using (_busyStack.GetToken())
                Items.RemoveRange(items);
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

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public virtual void Clear()
        {
            SelectedItem = default(TViewModel);

            using (_busyStack.GetToken())
                Items.Clear();
        }

        /// <summary>
        /// Determines whether this instance can clear.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can clear; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool CanClear()
        {
            return Items?.Count > 0 && !IsBusy;
        }

        public IEnumerable GetErrors(string propertyName) //TODO
        {
            return !string.IsNullOrEmpty(propertyName) && _errors.ContainsKey(propertyName)
              ? _errors[propertyName]
              : Enumerable.Empty<string>();
        }

        public IEnumerable<List<string>> GetErrors() //TODO
        {
            foreach (var key in _errors.Keys)
                yield return _errors[key];
        }

        protected virtual void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        protected void ClearErrors() //TODO
        {
            foreach (var propertyName in _errors.Keys.ToList())
            {
                _errors.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }

        protected void AddRule(object value, BaseValidationRule rule) //TODO
        {
            var propertyName = rule.PropertyName;
            var result = (Items: new List<BaseValidationRule>(), Value: value);

            if (_rules.ContainsKey(propertyName))
                result = _rules[propertyName];

            if (!result.Items.Contains(rule))
                result.Items.Add(rule);

            result.Value = value;

            _rules[propertyName] = result;
        }

        protected virtual void Validate(string propertyName) //TODO
        {
            if (string.IsNullOrEmpty(propertyName))
                ValidateAllInternal();
            else
                ValidateInternal(propertyName);
        }

        private void ValidateInternal(string propertyName, CultureInfo culture = null) //TODO
        {
            var current = culture ?? Thread.CurrentThread.CurrentCulture;

            if (_errors.ContainsKey(propertyName))
                _errors[propertyName].Clear();

            foreach (var item in _rules[propertyName].Items)
            {
                var result = item.Validate(_rules[propertyName].Value, current);
                if (_errors.ContainsKey(propertyName))
                    _errors[propertyName].Add(result.ErrorContent.ToString());
                else
                {
                    _errors.Add(propertyName, new List<string>()
                    {
                        result.ErrorContent.ToString(),
                    });
                }
            }

            OnErrorsChanged(propertyName);
        }

        private void ValidateAllInternal() //TODO
        {
            var current = Thread.CurrentThread.CurrentCulture;

            foreach (var key in _errors.Keys)
                ValidateInternal(key, current);
        }
    }
}
