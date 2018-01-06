using System;
using System.Collections.Generic;
using Maple.Localization.Properties;

namespace Maple.Core
{
    public abstract class BaseDataProvider<TViewModel, TPrimaryKeyType> : IDataProvider<TViewModel, TPrimaryKeyType>
        where TViewModel : class
    {
        private readonly IDictionary<TPrimaryKeyType, TViewModel> _cache;
        private readonly Func<TPrimaryKeyType, TViewModel> _viewModelFactory;

        protected BaseDataProvider(Func<TPrimaryKeyType, TViewModel> viewModelFactory, IDictionary<TPrimaryKeyType, TViewModel> cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache), $"{nameof(cache)} {Resources.IsRequired}");
            _viewModelFactory = viewModelFactory ?? throw new ArgumentNullException(nameof(viewModelFactory), $"{nameof(viewModelFactory)} {Resources.IsRequired}");
        }

        public void Chunk(IEnumerable<TPrimaryKeyType> Ids)
        {
            foreach (var id in Ids)
            {
                if (_cache.ContainsKey(id))
                    continue;

                _cache.Add(id, InternalGet(id));
            }
        }

        public TViewModel Get(TPrimaryKeyType id)
        {
            return _cache[id];
        }

        private TViewModel InternalGet(TPrimaryKeyType id)
        {
            return _viewModelFactory(id);
        }

        public void Clear()
        {
            _cache.Clear();
        }

        public void Remove(TPrimaryKeyType id)
        {
            _cache.Remove(id);
        }
    }
}
