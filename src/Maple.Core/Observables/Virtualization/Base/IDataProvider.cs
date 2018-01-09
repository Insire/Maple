using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maple.Core
{
    public interface IDataProvider<T, TPrimaryKeyType>
    {
        Task<T> Get(TPrimaryKeyType id);

        Task Chunk(IEnumerable<TPrimaryKeyType> Ids);

        void Clear();
        void Remove(TPrimaryKeyType id);
    }
}
