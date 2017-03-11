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
        private bool _suppressNotification = false;

        public RangeObservableCollection() : base()
        {
        }

        public RangeObservableCollection(IEnumerable<T> items) : this()
        {
            AddRange(items);
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            DispatcherFactory.Invoke(() =>
            {
                if (!_suppressNotification)
                    RaiseCollectionChanged(e);
            });
        }

        private void RaiseCollectionChanged(object param)
        {
            // We are in the creator thread, call the base implementation directly
            base.OnCollectionChanged((NotifyCollectionChangedEventArgs)param);
        }

        public virtual void AddRange(List<T> items)
        {
            AddRange(items.AsEnumerable());
        }

        public virtual void AddRange(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            _suppressNotification = true;

            foreach (var item in items)
                Add(item);

            _suppressNotification = false;

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public virtual void RemoveRange(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            _suppressNotification = true;

            foreach (var item in items)
                Remove(item);

            _suppressNotification = false;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public virtual void RemoveRange(IList items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            _suppressNotification = true;

            foreach (var item in items.Cast<T>())
                Remove(item);

            _suppressNotification = false;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public virtual void RemoveRange(IList<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            _suppressNotification = true;

            foreach (var item in items)
                Remove(item);

            _suppressNotification = false;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            DispatcherFactory.Invoke(() =>
            {
                if (!_suppressNotification)
                    RaisePropertyChanged(e);
            });
        }

        private void RaisePropertyChanged(object param)
        {
            // We are in the creator thread, call the base implementation directly
            base.OnPropertyChanged((PropertyChangedEventArgs)param);
        }
    }
}
