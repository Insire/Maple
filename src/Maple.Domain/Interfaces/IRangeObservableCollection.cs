﻿using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Maple.Domain
{
    public interface IRangeObservableCollection<T> : INotifyPropertyChanged, IList<T>, INotifyCollectionChanged
    {
        void AddRange(IEnumerable<T> items);

        void OnPropertyChanged([CallerMemberName] string propertyName = null);

        void RemoveRange(IEnumerable<T> items);
    }
}
