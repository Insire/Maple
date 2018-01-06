using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Maple.Core
{
    public class WeakReferenceCache<TPrimaryKeyType, TViewModel> : ICachingService<TPrimaryKeyType, TViewModel>
        where TViewModel : class
    {
        private readonly ConcurrentDictionary<TPrimaryKeyType, WeakReference<TViewModel>> _references;

        public WeakReferenceCache()
        {
            _references = new ConcurrentDictionary<TPrimaryKeyType, WeakReference<TViewModel>>();
        }

        public bool Add(TPrimaryKeyType key, TViewModel viewModel)
        {
            throw new NotImplementedException();
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
