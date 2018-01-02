using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Maple.Domain;

namespace Maple.Data
{
#pragma warning disable CA1012 // Abstract types should not have constructors
    public abstract class BaseRepository<T> : IMapleRepository<T>
#pragma warning restore CA1012 // Abstract types should not have constructors
        where T : class
    {
        protected IConnectionStringManager ConnectionStringManager { get; }

        public BaseRepository(IConnectionStringManager connectionStringManager)
        {
            ConnectionStringManager = connectionStringManager ?? throw new ArgumentNullException();
        }

        public async Task<IReadOnlyCollection<T>> GetAsync()
        {
            return await DBGenericActions.GetAllFromEntity<T>(ConnectionStringManager.Get()).ConfigureAwait(false);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await DBGenericActions.GetEntityByPK<T>(ConnectionStringManager.Get(), id).ConfigureAwait(false);
        }

        public async Task SaveAsync(T item)
        {
            await DBGenericActions.InsertOrUpdateEntity<T>(ConnectionStringManager.Get(), item).ConfigureAwait(false);
        }
    }
}
