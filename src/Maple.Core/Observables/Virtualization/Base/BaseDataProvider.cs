using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Maple.Localization.Properties;

namespace Maple.Core
{
    public abstract class BaseDataProvider<TViewModel, TPrimaryKeyType> : IDataProvider<TViewModel, TPrimaryKeyType>
        where TViewModel : class
    {
        private readonly ICachingService<TPrimaryKeyType, TViewModel> _cache;
        private readonly Func<TPrimaryKeyType, Task<TViewModel>> _viewModelFactory;

        protected BaseDataProvider(Func<TPrimaryKeyType, Task<TViewModel>> viewModelFactory, ICachingService<TPrimaryKeyType, TViewModel> cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache), $"{nameof(cache)} {Resources.IsRequired}");
            _viewModelFactory = viewModelFactory ?? throw new ArgumentNullException(nameof(viewModelFactory), $"{nameof(viewModelFactory)} {Resources.IsRequired}");
        }

        public async Task Chunk(IEnumerable<TPrimaryKeyType> Ids)
        {
            foreach (var id in Ids)
            {
                if (_cache.Contains(id))
                    continue;

                _cache.Add(id, await InternalGet(id).ConfigureAwait(false));
            }
        }

        public async Task<TViewModel> Get(TPrimaryKeyType key)
        {
            if (_cache.TryGetValue(key, out var result))
                return result;

            return await InternalGet(key).ConfigureAwait(false);
        }

        private async Task<TViewModel> InternalGet(TPrimaryKeyType key)
        {
            return await _viewModelFactory(key).ConfigureAwait(false);
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
