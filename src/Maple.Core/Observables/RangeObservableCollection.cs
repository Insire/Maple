using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple.Core
{
    public class RangeObservableCollection<T> : ObservableCollection<T>, IRangeObservableCollection<T>
    {
        private bool _suppressNotification;
        private readonly BusyStack _busyStack;

        public RangeObservableCollection()
        {
            _busyStack = new BusyStack(updatePending => _suppressNotification = updatePending);
        }

        public RangeObservableCollection(IEnumerable<T> items) : this()
        {
            AddRange(items);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Collections.ObjectModel.ObservableCollection`1.CollectionChanged" /> event with the provided arguments.
        /// </summary>
        /// <param name="e">Arguments of the event being raised.</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!_suppressNotification)
                RaiseCollectionChanged(e);
        }

        /// <summary>
        /// Raises the collection changed.
        /// </summary>
        /// <param name="param">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs param)
        {
            base.OnCollectionChanged(param);
        }

        public virtual void AddRange(IList items)
        {
            AddRange(items.Cast<T>());
        }

        public virtual void AddRange(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items), $"{nameof(items)} {Resources.IsRequired}");

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
                throw new ArgumentNullException(nameof(items), $"{nameof(items)} {Resources.IsRequired}");

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
                throw new ArgumentNullException(nameof(items), $"{nameof(items)} {Resources.IsRequired}");

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
                throw new ArgumentNullException(nameof(items), $"{nameof(items)} {Resources.IsRequired}");

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

        public void AddRange(IList<T> items)
        {
            AddRange((IEnumerable<T>)items);
        }
    }
}
