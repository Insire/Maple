using System;
using System.Collections.Concurrent;

namespace Maple.Core
{
    public sealed class WeakReferenceCache<TKey, TObject> : ICachingService<TKey, TObject>
        where TObject : class
    {
        private readonly ConcurrentDictionary<TKey, WeakReference<TObject>> _references;

        public WeakReferenceCache()
        {
            _references = new ConcurrentDictionary<TKey, WeakReference<TObject>>();
        }

        public bool Add(TKey key, TObject item)
        {
            return Add(key, item, false);
        }

        public bool Add(TKey key, TObject item, bool isStrongReference)
        {
            if (item is IDisposable)
                throw new ArgumentException("can't accept IDisposeable, since we dont know when the object will be disposed and we cant call Dispose on it");

            _references.AddOrUpdate(key, new WeakReference<TObject>(item), (existingKey, existingEntry) => new WeakReference<TObject>(item));


            return true;
            // Consider
            // And finally a word of warning: weak references aren't a guaranteed profit for application performance.
            // In most cases, they will make an algorithm more performant than when using very large local variables.
            // But it's not guaranteed, and in some cases it could produce noticeable overhead(for example when the huge object is a data structure consisting of many smaller objects with references to each other,
            // and a WeakRefence to the data structure turns all those internal refernces to weak references, this might incur garbage collector overhead,
            // because every reference has to be checked to decide if the object as a whole can be recovered or deleted).
            // So the best advice with weak references is: profile or benchmark it, to make sure that you choose the best solution for your specific situation.
        }

        public bool TryGetValue(TKey key, out TObject item)
        {
            item = default;

            if (_references.TryGetValue(key, out var reference))
                return reference.TryGetTarget(out item);

            return false;
        }

        public void Clear()
        {
            _references.Clear();
        }

        public bool Contains(TKey key)
        {
            return _references.ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            return _references.TryRemove(key, out _);
        }
    }
}
