using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using Maple.Localization.Properties;

namespace Maple.Core
{
    public abstract class MemoryCacheService<TPrimaryKeyType, TViewModel> : ICachingService<TPrimaryKeyType, TViewModel>
        where TViewModel : class
    {
        private readonly HashSet<TPrimaryKeyType> _trackedEntries;
        private readonly MemoryCache _cache;
        private readonly CacheItemPolicy _policy;
        private readonly IVirtualizedViewModel _virtualizationProvider;
        private readonly string _cacheKeyPart;

        private MemoryCacheService()
        {
            _cacheKeyPart = typeof(TViewModel).FullName;
            _trackedEntries = new HashSet<TPrimaryKeyType>();
            _policy = new CacheItemPolicy()
            {
                SlidingExpiration = TimeSpan.FromSeconds(3),
                Priority = CacheItemPriority.Default,
                RemovedCallback = new CacheEntryRemovedCallback(CacheRemovedCallback)
            };
        }

        protected MemoryCacheService(MemoryCache cache, IVirtualizedViewModel virtualizationProvider)
            : this()
        {
            _virtualizationProvider = virtualizationProvider ?? throw new ArgumentNullException(nameof(virtualizationProvider), $"{nameof(virtualizationProvider)} {Resources.IsRequired}");
            _cache = cache ?? throw new ArgumentNullException(nameof(cache), $"{nameof(cache)} {Resources.IsRequired}");
        }

        public bool Add(TPrimaryKeyType key, TViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel), $"{nameof(viewModel)} {Resources.IsRequired}");

            var result = _cache.Add(new CacheItem(_cacheKeyPart + key, viewModel), _policy);

            if (result)
                _trackedEntries.Add(key);

            return result;
        }

        public bool Contains(TPrimaryKeyType key)
        {
            return _cache.Contains(GetInternalKey(key));
        }

        public void Clear()
        {
            foreach (var entry in _trackedEntries)
                Remove(entry);

            _trackedEntries.Clear();
        }

        public bool Remove(TPrimaryKeyType key)
        {
            var result = _cache.Remove(GetInternalKey(key)) is TViewModel;

            if (result)
                _trackedEntries.Remove(key);

            return result;
        }

        public abstract void CacheRemovedCallback(CacheEntryRemovedArguments arguments);
        //{
        //    _virtualizationProvider.DeflateItem(arguments.CacheItem.Value);
        //}


        private string GetInternalKey(TPrimaryKeyType key)
        {
            return _cacheKeyPart + key;
        }
    }
}
