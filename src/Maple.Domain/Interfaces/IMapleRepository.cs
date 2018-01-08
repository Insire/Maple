using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maple.Domain
{
    public interface IMapleRepository<TModel, TKeyDataType>
        where TModel : class, IBaseObject<TKeyDataType>
    {
        Task<List<TModel>> GetAsync();
        Task<TModel> GetByIdAsync(TKeyDataType Id);

        Task<List<TKeyDataType>> GetKeysAsync();
        Task<int> GetEntryCountAsync();

        Task SaveAsync(TModel item);
    }
}