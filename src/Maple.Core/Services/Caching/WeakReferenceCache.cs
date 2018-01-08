using System;
using System.Collections.Concurrent;

namespace Maple.Core
{
    public sealed class WeakReferenceCache<TPrimaryKeyType, TViewModel> : ICachingService<TPrimaryKeyType, TViewModel>
        where TViewModel : class
    {
        private readonly ConcurrentDictionary<TPrimaryKeyType, WeakReference<TViewModel>> _references;

        public WeakReferenceCache()
        {
            _references = new ConcurrentDictionary<TPrimaryKeyType, WeakReference<TViewModel>>();
        }

        public bool Add(TPrimaryKeyType key, TViewModel viewModel)
        {
            throw new NotImplementedException(); // can't accept IDisposeable, since we dont know when the object will be disposed and we cant call Dispose on it

            // Consider
            // And finally a word of warning: weak references aren't a guaranteed profit for application performance.
            // In most cases, they will make an algorithm more performant than when using very large local variables.
            // But it's not guaranteed, and in some cases it could produce noticeable overhead(for example when the huge object is a data structure consisting of many smaller objects with references to each other,
            // and a WeakRefence to the data structure turns all those internal refernces to weak references, this might incur garbage collector overhead,
            // because every reference has to be checked to decide if the object as a whole can be recovered or deleted).
            // So the best advice with weak references is: profile or benchmark it, to make sure that you choose the best solution for your specific situation.
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(TPrimaryKeyType key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(TPrimaryKeyType key)
        {
            throw new NotImplementedException();
        }
    }
}
