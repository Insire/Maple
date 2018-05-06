using System.Collections.Generic;

namespace Maple.Core
{
    public interface IVirtualizedListViewModel
    {
        void ExtendItems(IEnumerable<object> items);
        void DeflateItem(object item);
    }
}
