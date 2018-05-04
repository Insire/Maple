using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

using Maple.Domain;

namespace Maple.Data
{
    public abstract class EntityFrameworkRepository<TContext> : EntityFrameworkReadOnlyRepository<TContext>, IRepository
        where TContext : DbContext
    {
        public EntityFrameworkRepository(TContext context)
            : base(context)
        {
        }

        public virtual void Create<TEntity>(TEntity entity, string createdBy = null)
            where TEntity : class, IEntity
        {
            entity.CreatedOn = DateTime.UtcNow;
            entity.CreatedBy = createdBy;
            context.Set<TEntity>().Add(entity);
        }

        public virtual void Update<TEntity>(TEntity entity, string modifiedBy = null)
            where TEntity : class, IEntity
        {
            entity.UpdatedOn = DateTime.UtcNow;
            entity.UpdatedBy = modifiedBy;
            context.Set<TEntity>().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete<TEntity>(object id)
            where TEntity : class, IEntity
        {
            var entity = context.Set<TEntity>().Find(id);
            Delete(entity);
        }

        public virtual void Delete<TEntity>(TEntity entity)
            where TEntity : class, IEntity
        {
            var dbSet = context.Set<TEntity>();
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        protected virtual void ThrowEnhancedValidationException(DbEntityValidationException e)
        {
            var errorMessages = e.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

            var fullErrorMessage = string.Join("; ", errorMessages);
            var exceptionMessage = string.Concat(e.Message, " The validation errors are: ", fullErrorMessage);

            throw new DbEntityValidationException(exceptionMessage, e.EntityValidationErrors);
        }
    }
}