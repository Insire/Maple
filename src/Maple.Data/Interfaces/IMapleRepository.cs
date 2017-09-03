using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maple.Data
{
    public interface IMapleRepository<T>
        where T : BaseObject
    {
        Task<List<T>> GetAsync();
        Task<T> GetByIdAsync(int Id);
        void Save(T item);
    }
}