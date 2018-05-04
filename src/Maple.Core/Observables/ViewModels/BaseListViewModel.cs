using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

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
        private TViewModel _selectedItem;
        public TViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                SetValue(ref _selectedItem, value,
                    () => Messenger.Publish(new ViewModelSelectionChangingMessage<TViewModel>(Items, _selectedItem)),
                    () => Messenger.Publish(new ViewModelSelectionChangedMessage<TViewModel>(Items, _selectedItem)));
            }
        }

        protected ObservableCollection<TViewModel> Items { get; }
        public ReadOnlyCollection<TViewModel> ReadOnlyItems { get; }

        public TViewModel this[int index]
        {
            get { return Items[index]; }
        }

        public ICommand RemoveRangeCommand { get; private set; }
        public ICommand RemoveCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }

        protected BaseListViewModel(IMessenger messenger)
            : base(messenger)
        {
            Items = new ObservableCollection<TViewModel>();
            ReadOnlyItems = new ReadOnlyCollection<TViewModel>(Items);

            RemoveCommand = new RelayCommand<TViewModel>(Remove, CanRemove);
            RemoveRangeCommand = new RelayCommand<IList>(RemoveRange, CanRemoveRange);
            ClearCommand = new RelayCommand(Clear, CanClear);
        }

        public virtual void Add(TViewModel item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            using (BusyStack.GetToken())
                Items.Add(item);
        }

        public virtual void AddRange(IEnumerable<TViewModel> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            using (BusyStack.GetToken())
                Items.ForEach(p => Add(p));
        }

        public virtual void Remove(TViewModel item)
        {
            using (BusyStack.GetToken())
                Items.Remove(item);
        }

        public virtual void RemoveRange(IEnumerable<TViewModel> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            using (BusyStack.GetToken())
                Items.ForEach(p => Remove(p));
        }

        public virtual void RemoveRange(IList items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            using (BusyStack.GetToken())
                Items.ForEach(p => Remove(p));
        }

        public virtual bool CanRemove(TViewModel item)
        {
            return CanClear() && item != null && Items.Contains(item);
        }

        public virtual bool CanRemoveRange(IEnumerable<TViewModel> items)
        {
            return CanClear() && items != null && items.Any(p => Items.Contains(p));
        }

        public virtual bool CanRemoveRange(IList items)
        {
            return items == null ? false : CanRemoveRange(items.Cast<TViewModel>());
        }

        public virtual void Clear()
        {
            SelectedItem = default(TViewModel);

            using (BusyStack.GetToken())
                Items.Clear();
        }

        public virtual bool CanClear()
        {
            return Items?.Count > 0 && !IsBusy;
        }
    }
}
