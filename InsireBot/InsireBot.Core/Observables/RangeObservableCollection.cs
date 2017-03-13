using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Maple.Core
{
    public class RangeObservableCollection<T> : ObservableCollection<T>, INotifyPropertyChanged
    {
        private bool _suppressNotification;
        private readonly BusyStack _busyStack;

        public RangeObservableCollection()
            : base()
        {
            _busyStack = new BusyStack();
            _busyStack.OnChanged += (updatePending) => _suppressNotification = updatePending;
        }

        public RangeObservableCollection(IEnumerable<T> items) : this()
        {
            AddRange(items);
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!_suppressNotification)
                RaiseCollectionChanged(e);
        }

        private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs param)
        {
            base.OnCollectionChanged(param);
        }

        public virtual void AddRange(List<T> items)
        {
            AddRange(items.AsEnumerable());
        }

        public virtual void AddRange(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            using (_busyStack.GetToken())
            {
                foreach (var item in items)
                    Add(item);
            }

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public virtual void RemoveRange(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            using (_busyStack.GetToken())
            {
                foreach (var item in items)
                    Remove(item);
            }

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public virtual void RemoveRange(IList items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            using (_busyStack.GetToken())
            {
                foreach (var item in items.Cast<T>())
                    Remove(item);
            }

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public virtual void RemoveRange(IList<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            using (_busyStack.GetToken())
            {
                foreach (var item in items)
                    Remove(item);
            }

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(e);
        }

        private void RaisePropertyChanged(PropertyChangedEventArgs param)
        {
            base.OnPropertyChanged(param);
        }
    }
}
