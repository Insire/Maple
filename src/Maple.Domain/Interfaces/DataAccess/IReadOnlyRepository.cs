using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Maple.Domain
{
    public interface IReadOnlyRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        Task<TEntity> ReadAsync(TKey key, CancellationToken token);

        Task<ICollection<TEntity>> ReadAsync(Expression<Func<TEntity, bool>> filter, string[] propertyNames, int index, int count, CancellationToken token);
        Task<ICollection<TEntity>> ReadAsync(CancellationToken token);
        Task<ICollection<TEntity>> ReadAsync();
        Task<TEntity> ReadAsync(TKey key);
        Task<ICollection<TEntity>> ReadAsync(Expression<Func<TEntity, bool>> filter, string[] propertyNames, int index, int count);
    }
}
