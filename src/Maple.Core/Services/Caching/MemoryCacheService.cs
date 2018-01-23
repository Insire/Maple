using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using Maple.Localization.Properties;

namespace Maple.Core
{
    public abstract class MemoryCacheService<TKey, TObject> : ICachingService<TKey, TObject>
        where TObject : class
    {
        private readonly HashSet<TKey> _trackedEntries;
        private readonly MemoryCache _cache;
        private readonly CacheItemPolicy _policy;
        private readonly IVirtualizedListViewModel _virtualizationProvider;
        private readonly string _cacheKeyPart;

        private MemoryCacheService()
        {
            _cacheKeyPart = typeof(TObject).FullName;
            _trackedEntries = new HashSet<TKey>();
            _policy = new CacheItemPolicy()
            {
                SlidingExpiration = TimeSpan.FromSeconds(3),
                Priority = CacheItemPriority.Default,
                RemovedCallback = new CacheEntryRemovedCallback(CacheRemovedCallback)
            };
        }

        protected MemoryCacheService(MemoryCache cache, IVirtualizedListViewModel virtualizationProvider)
            : this()
        {
            _virtualizationProvider = virtualizationProvider ?? throw new ArgumentNullException(nameof(virtualizationProvider), $"{nameof(virtualizationProvider)} {Resources.IsRequired}");
            _cache = cache ?? throw new ArgumentNullException(nameof(cache), $"{nameof(cache)} {Resources.IsRequired}");
        }

        public bool Add(TKey key, TObject viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel), $"{nameof(viewModel)} {Resources.IsRequired}");

            var result = _cache.Add(new CacheItem(GetInternalKey(key), viewModel), _policy);

            if (result)
                _trackedEntries.Add(key);

            return result;
        }

        public bool Contains(TKey key)
        {
            return _cache.Contains(GetInternalKey(key));
        }

        public void Clear()
        {
            foreach (var entry in _trackedEntries)
                Remove(entry);

            _trackedEntries.Clear();
        }

        public bool Remove(TKey key)
        {
            var result = _cache.Remove(GetInternalKey(key)) is TObject;

            if (result)
                _trackedEntries.Remove(key);

            return result;
        }

        public abstract void CacheRemovedCallback(CacheEntryRemovedArguments arguments);
        //{
        //    _virtualizationProvider.DeflateItem(arguments.CacheItem.Value);
        //}


        private string GetInternalKey(TKey key)
        {
            return _cacheKeyPart + key;
        }

        public bool TryGetValue(TKey key, out TObject item)
        {
            item = (TObject)_cache.Get(GetInternalKey(key));

            return true;
        }

        // hm, speed?
        public bool TryGetValues(out IEnumerable<TObject> items, params TKey[] keys)
        {
            items = _cache.GetValues(keys.Select(key => GetInternalKey(key))).Select(p => p.Value).Cast<TObject>();

            return true;
        }
    }
}
