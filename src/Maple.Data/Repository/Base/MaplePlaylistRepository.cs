using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Maple.Interfaces;

namespace Maple.Data
{
    public abstract class MaplePlaylistRepository<T> : IMapleRepository<T>
        where T : class, IBaseObject
    {
        public void Save(T item)
        {
            using (var context = new PlaylistContext())
                SaveInternal(item, context);
        }

        protected abstract DbSet<T> GetEntities(PlaylistContext context);

        protected virtual void SaveInternal(T item, PlaylistContext context)
        {
            if (item.IsNew)
                Create(item, context);
            else
            {
                if (item.IsDeleted)
                {
                    context.Database.Log = (message) => Debug.WriteLine(message);
                    Delete(item, context);
                }
                else
                    Update(item, context);
            }

            context.SaveChanges();
        }

        protected virtual void Delete(T item, PlaylistContext context)
        {
            context.Set<T>().Remove(item);
        }

        protected virtual void Create(T item, PlaylistContext context)
        {
            item.CreatedBy = System.Security.Principal.WindowsIdentity.GetCurrent().User.Value;
            item.CreatedOn = DateTime.UtcNow;

            context.Set<T>().Add(item);
        }

        protected virtual void Update(T item, PlaylistContext context)
        {
            var entity = GetEntities(context).Find(item.Id);

            if (entity == null)
                return;

            item.UpdatedOn = DateTime.UtcNow;
            item.UpdatedBy = System.Security.Principal.WindowsIdentity.GetCurrent().User.Value;


            context.Entry(entity).CurrentValues.SetValues(item);
        }

        public Task<T> GetByIdAsync(int Id)
        {
            return Task.Run(() =>
            {
                using (var context = new PlaylistContext())
                    return GetByIdInternalAsync(Id, context);
            });
        }

        protected virtual T GetByIdInternalAsync(int id, PlaylistContext context)
        {
            return GetEntities(context).FirstOrDefault(p => p.Id == id);
        }

        public Task<IReadOnlyCollection<T>> GetAsync()
        {
            return Task.Run(() =>
            {
                using (var context = new PlaylistContext())
                    return GetInternalAsync(context);
            });
        }

        protected virtual IReadOnlyCollection<T> GetInternalAsync(PlaylistContext context)
        {
            return GetEntities(context).ToList();
        }
    }
}