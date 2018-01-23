using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Windows.Data;
using System.Windows.Threading;
using Maple.Localization.Properties;

namespace Maple.Core
{
    public class VirtualizingCollectionViewSource : ListCollectionView
    {
        private readonly IVirtualizedListViewModel _sponsor;
        private readonly HashSet<object> _deferredItems;
        private readonly MemoryCache _cache;
        private readonly CacheItemPolicy _policy;

        private bool _isDeferred;

        public VirtualizingCollectionViewSource(ViewModelServiceContainer container, IList list)
            : base(list)
        {
            _cache = container.Cache ?? throw new ArgumentNullException(nameof(container.Cache), $"{nameof(container.Cache)} {Resources.IsRequired}");
            _policy = container.CacheItemPolicy ?? throw new ArgumentNullException(nameof(container.CacheItemPolicy), $"{nameof(container.CacheItemPolicy)} {Resources.IsRequired}");

            _deferredItems = new HashSet<object>();
            _sponsor = list as IVirtualizedListViewModel;
        }

        public void CacheRemovedCallback(CacheEntryRemovedArguments arguments)
        {
            _sponsor.DeflateItem(arguments.CacheItem.Value);
        }

        public override object GetItemAt(int index)
        {
            if (!_isDeferred)
            {
                _deferredItems.Clear();

                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)LoadDeferredItems);

                _isDeferred = true;
            }

            var item = base.GetItemAt(index);
            if (!_deferredItems.Contains(item))
                _deferredItems.Add(item);

            return item;
        }

        private void LoadDeferredItems()
        {
            var uniqueSet = new HashSet<object>();
            foreach (var item in _deferredItems)
            {
                var hashCode = item.GetHashCode();
                if (!_cache.Contains(hashCode.ToString()))
                    uniqueSet.Add(item);

                _cache.Add(new CacheItem(hashCode.ToString(), item), _policy);
            }

            _sponsor.ExtendItems(uniqueSet);
            _isDeferred = false;
        }
    }
}
