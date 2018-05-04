using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Maple.Domain;

namespace Maple.Data
{
    public abstract class EntityFrameworkReadOnlyRepository<TContext> : IReadOnlyRepository
        where TContext : DbContext
    {
        protected readonly TContext context;

        public EntityFrameworkReadOnlyRepository(TContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        protected virtual IQueryable<TEntity> GetQueryable<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null, int? skip = null, int? take = null)
            where TEntity : class, IEntity
        {
            includeProperties = includeProperties ?? string.Empty;
            IQueryable<TEntity> query = context.Set<TEntity>();

            if (filter != null)
                query = query.Where(filter);

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);

            if (orderBy != null)
                query = orderBy(query);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (take.HasValue)
                query = query.Take(take.Value);

            return query;
        }

        public virtual IEnumerable<TEntity> GetAll<TEntity>(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class, IEntity
        {
            return GetQueryable<TEntity>(null, orderBy, includeProperties, skip, take).ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class, IEntity
        {
            return await GetQueryable<TEntity>(null, orderBy, includeProperties, skip, take).ToListAsync();
        }

        public virtual IEnumerable<TEntity> Get<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class, IEntity
        {
            return GetQueryable<TEntity>(filter, orderBy, includeProperties, skip, take).ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class, IEntity
        {
            return await GetQueryable<TEntity>(filter, orderBy, includeProperties, skip, take).ToListAsync();
        }

        public virtual TEntity GetOne<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = "")
            where TEntity : class, IEntity
        {
            return GetQueryable<TEntity>(filter, null, includeProperties).SingleOrDefault();
        }

        public virtual async Task<TEntity> GetOneAsync<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = null)
            where TEntity : class, IEntity
        {
            return await GetQueryable<TEntity>(filter, null, includeProperties).SingleOrDefaultAsync();
        }

        public virtual TEntity GetFirst<TEntity>(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties = "")
           where TEntity : class, IEntity
        {
            return GetQueryable<TEntity>(filter, orderBy, includeProperties).FirstOrDefault();
        }

        public virtual async Task<TEntity> GetFirstAsync<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null)
            where TEntity : class, IEntity
        {
            return await GetQueryable<TEntity>(filter, orderBy, includeProperties).FirstOrDefaultAsync();
        }

        public virtual TEntity GetById<TEntity>(object id)
            where TEntity : class, IEntity
        {
            return context.Set<TEntity>().Find(id);
        }

        public virtual Task<TEntity> GetByIdAsync<TEntity>(object id)
            where TEntity : class, IEntity
        {
            return context.Set<TEntity>().FindAsync(id);
        }

        public Task<List<IEntity>> GetByIdsAsync<TEntity>(ICollection<object> ids)
            where TEntity : class, IEntity
        {
            return GetQueryable<IEntity>(p => ids.Contains(p.Id)).ToListAsync();
        }

        public virtual int GetCount<TEntity>(Expression<Func<TEntity, bool>> filter = null)
            where TEntity : class, IEntity
        {
            return GetQueryable<TEntity>(filter).Count();
        }

        public virtual Task<int> GetCountAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null)
            where TEntity : class, IEntity
        {
            return GetQueryable<TEntity>(filter).CountAsync();
        }

        public virtual bool GetExists<TEntity>(Expression<Func<TEntity, bool>> filter = null)
            where TEntity : class, IEntity
        {
            return GetQueryable<TEntity>(filter).Any();
        }

        public virtual Task<bool> GetExistsAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null)
            where TEntity : class, IEntity
        {
            return GetQueryable<TEntity>(filter).AnyAsync();
        }
    }
}