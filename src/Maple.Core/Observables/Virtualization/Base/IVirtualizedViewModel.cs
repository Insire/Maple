using System.Collections.Generic;

namespace Maple.Core
{
    public interface IVirtualizedViewModel
    {
        void ExtendItems(IEnumerable<object> items);
        void DeflateItem(object item);
    }
}
