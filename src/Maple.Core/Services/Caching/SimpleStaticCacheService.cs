using System.Collections.Generic;

namespace Maple.Core
{
    public sealed class SimpleStaticCacheService<TPrimaryKeyType, TViewModel> : Dictionary<TPrimaryKeyType, TViewModel>, ICachingService<TPrimaryKeyType, TViewModel>
    {
        public bool Contains(TPrimaryKeyType key)
        {
            return ContainsKey(key);
        }

        bool ICachingService<TPrimaryKeyType, TViewModel>.Add(TPrimaryKeyType key, TViewModel viewModel)
        {
            Add(key, viewModel);

            return true;
        }
    }
}
