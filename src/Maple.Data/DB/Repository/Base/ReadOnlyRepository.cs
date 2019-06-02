using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            if (unitOfWork is UnitOfWork uow)
            {
                Set = uow.Context.Set<TEntity>();
                uow.Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            }
            else
            {
                throw new ArgumentException(nameof(unitOfWork) + " is not derived from " + typeof(UnitOfWork).FullName);
            }

            Name = GetType().Name;
        }

        public TEntity Read(TKey key)
        {
            return Set.Find(key);
        }

        public Task<TEntity> ReadAsync(TKey key)
        {
            return Set.FindAsync(key);
        }

        public ICollection<TEntity> Read(Expression<Func<TEntity, bool>> filter, string[] propertyNames, int index, int count)
        {
            return ReadInternal(Set, filter, propertyNames, index, count)
                .ToList()
                .AsReadOnly();
        }

        public async Task<ICollection<TEntity>> ReadAsync(Expression<Func<TEntity, bool>> filter, string[] propertyNames, int index, int count)
        {
            var result = await ReadInternal(Set, filter, propertyNames, index, count)
                                .ToListAsync()
                                .ConfigureAwait(false);

            return result.AsReadOnly();
        }

        private static IQueryable<TEntity> ReadInternal(IQueryable<TEntity> query, Expression<Func<TEntity, bool>> filter, string[] propertyNames, int index, int count)
        {
            if (!(propertyNames is null))
            {
                for (var i = 0; i < propertyNames.Length; i++)
                {
                    var name = propertyNames[i];
                    if (!string.IsNullOrEmpty(name))
                        query = query.Include(name);
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
