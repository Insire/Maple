using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Maple.Domain;
using Microsoft.EntityFrameworkCore;

namespace Maple.Data
{
    public abstract class ReadOnlyRepository<TEntity, TKey> : IReadOnlyRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        protected readonly DbSet<TEntity> Set;
        protected readonly string Name;

        protected ReadOnlyRepository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork is UnitOfWork<PlaylistContext> uow)
            {
                Set = uow.Context.Set<TEntity>();
                uow.Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            }
            else
            {
                throw new ArgumentException(nameof(unitOfWork) + " is not derived from " + typeof(UnitOfWork<PlaylistContext>).FullName);
            }

            Name = GetType().Name;
        }

        public Task<TEntity> ReadAsync(TKey key)
        {
            return Set.FindAsync(key, CancellationToken.None);
        }

        public Task<TEntity> ReadAsync(TKey key, CancellationToken token)
        {
            return Set.FindAsync(key, token);
        }

        public virtual Task<ICollection<TEntity>> ReadAsync()
        {
            return ReadAsync(CancellationToken.None);
        }

        public virtual async Task<ICollection<TEntity>> ReadAsync(CancellationToken token)
        {
            var result = await ReadInternal(Set, null, null, 0, 0)
                                .ToListAsync(token)
                                .ConfigureAwait(false);

            return result.AsReadOnly();
        }

        public virtual Task<ICollection<TEntity>> ReadAsync(Expression<Func<TEntity, bool>> filter, string[] propertyNames, int index, int count)
        {
            return ReadAsync(filter, propertyNames, index, count, CancellationToken.None);
        }

        public virtual async Task<ICollection<TEntity>> ReadAsync(Expression<Func<TEntity, bool>> filter, string[] propertyNames, int index, int count, CancellationToken token)
        {
            var result = await ReadInternal(Set, filter, propertyNames, index, count)
                                .ToListAsync(token)
                                .ConfigureAwait(false);

            return result.AsReadOnly();
        }

        protected IQueryable<TEntity> ReadInternal(Expression<Func<TEntity, bool>> filter, string[] propertyNames, int index, int count)
        {
            return ReadInternal(Set, filter, propertyNames, index, count);
        }

        private static IQueryable<TEntity> ReadInternal(IQueryable<TEntity> query, Expression<Func<TEntity, bool>> filter, string[] propertyNames, int index, int count)
        {
            if (!(propertyNames is null))
            {
                for (var i = 0; i < propertyNames.Length; i++)
                {
                    var name = propertyNames[i];
                    if (!string.IsNullOrEmpty(name))
                    {
                        query = query.Include(name);
                    }
                    else
                        throw new ArgumentException(nameof(propertyNames) + " contains an empty PropertyName.");
                }
            }

            if (!(filter is null))
                query = query.Where(filter);

            query = query.OrderBy(p => p.Id);

            if (index > 0)
                query = query.Skip(index);

            if (count > 0)
                query = query.Take(count);

            return query;
        }
    }
}
