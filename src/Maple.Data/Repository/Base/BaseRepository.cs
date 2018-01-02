using System.Collections.Generic;
using System.Threading.Tasks;
using Maple.Domain;

namespace Maple.Data
{
    public abstract class BaseRepository<T> : IMapleRepository<T>
        where T : class
    {
        public async Task<IReadOnlyCollection<T>> GetAsync()
        {
            return await DBGenericActions.GetAllFromEntity<T>().ConfigureAwait(false);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await DBGenericActions.GetEntityByPK<T>(id).ConfigureAwait(false);
        }

        public async Task SaveAsync(T item)
        {
            await DBGenericActions.InsertOrUpdateEntity<T>(item).ConfigureAwait(false);
        }
    }
}
