using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maple.Domain
{
    public interface IMapleRepository<T>
        where T : class
    {
        Task<IReadOnlyCollection<T>> GetAsync();
        Task<T> GetByIdAsync(int id);
        Task SaveAsync(T item);
    }
}