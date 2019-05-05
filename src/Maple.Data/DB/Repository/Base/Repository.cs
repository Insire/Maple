using Maple.Domain;

namespace Maple.Data
{
    public abstract class Repository<TEntity, TKey> : ReadOnlyRepository<TEntity, TKey>, IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        protected Repository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public void Create(TEntity entity)
        {
            Set.Add(entity);
        }

        public void Delete(TEntity entity)
        {
            Set.Remove(entity);
        }

        public void Update(TEntity entity)
        {
            if (entity.IsNew)
            {
                Create(entity);
            }
            else
            {
                if (entity.IsDeleted)
                {
                    Delete(entity);
                }
                else
                {
                    Set.Update(entity);
                }
            }
        }
    }
}
