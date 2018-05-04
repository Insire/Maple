using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Maple.Domain
{
    public interface IReadOnlyRepository
    {
        IEnumerable<TEntity> GetAll<TEntity>(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class, IEntity;

        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class, IEntity;

        IEnumerable<TEntity> Get<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class, IEntity;

        Task<IEnumerable<TEntity>> GetAsync<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class, IEntity;

        TEntity GetOne<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = null)
            where TEntity : class, IEntity;

        Task<TEntity> GetOneAsync<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = null)
            where TEntity : class, IEntity;

        TEntity GetFirst<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null)
            where TEntity : class, IEntity;

        Task<TEntity> GetFirstAsync<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null)
            where TEntity : class, IEntity;

        TEntity GetById<TEntity>(object id)
            where TEntity : class, IEntity;

        Task<TEntity> GetByIdAsync<TEntity>(object id)
            where TEntity : class, IEntity;

        Task<List<IEntity>> GetByIdsAsync<TEntity>(ICollection<object> ids)
            where TEntity : class, IEntity;

        int GetCount<TEntity>(Expression<Func<TEntity, bool>> filter = null)
            where TEntity : class, IEntity;

        Task<int> GetCountAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null)
            where TEntity : class, IEntity;

        bool GetExists<TEntity>(Expression<Func<TEntity, bool>> filter = null)
            where TEntity : class, IEntity;

        Task<bool> GetExistsAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null)
            where TEntity : class, IEntity;
    }
}
