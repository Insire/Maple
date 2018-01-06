using System.Collections.Generic;

namespace Maple.Core
{
    public interface IDataProvider<T, TPrimaryKeyType>
    {
        T Get(TPrimaryKeyType id);

        void Chunk(IEnumerable<TPrimaryKeyType> Ids);

        void Clear();
        void Remove(TPrimaryKeyType id);
    }
}
