using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple.Core
{
    public abstract class BaseDataProvider<TViewModel, TPrimaryKeyType> : IDataProvider<TViewModel, TPrimaryKeyType>
        where TViewModel : class
    {
        private readonly ICachingService<TPrimaryKeyType, TViewModel> _cache;

        protected BaseDataProvider(IRepository mapleRepository, ICachingService<TPrimaryKeyType, TViewModel> cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache), $"{nameof(cache)} {Resources.IsRequired}");
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
            throw new NotImplementedException();

            // check cache for entry
            // ask repository for entry
            // wrap result with mapper
            // return result
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
