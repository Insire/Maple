using System.Collections.Generic;

namespace Maple.Core
{
    public interface IDataVirtualizationItemProvider
    {
        void ExtendItems(IEnumerable<object> items);
        void DeflateItem(object item);
    }
}
