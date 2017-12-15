using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maple.Domain
{
    public interface IMapleRepository<T>
        where T : class, IBaseObject
    {
        Task<IReadOnlyCollection<T>> GetAsync();
        Task<T> GetByIdAsync(int Id);
        void Save(T item);
    }
}