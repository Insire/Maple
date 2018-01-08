using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Maple.Domain;

namespace Maple.Data
{
    public abstract class MaplePlaylistRepository<TModel, TKeyDataType> : IMapleRepository<TModel, TKeyDataType>
        where TModel : class, IBaseModel<TKeyDataType>
    {
        public async Task SaveAsync(TModel item)
        {
            using (var context = new PlaylistContext())
                await SaveInternal(item, context).ConfigureAwait(false);
        }

        public async Task<TModel> GetByIdAsync(TKeyDataType Id)
        {
            using (var context = new PlaylistContext())
                return await GetByIdInternalAsync(Id, context).ConfigureAwait(false);
        }

        public async Task<List<TModel>> GetAsync()
        {
            using (var context = new PlaylistContext())
                return await GetInternalAsync(context).ConfigureAwait(false);
        }

        public async Task<List<TKeyDataType>> GetKeysAsync()
        {
            using (var context = new PlaylistContext())
                return await GetEntities(context).Select(p => p.Id).ToListAsync().ConfigureAwait(false);
        }

        public async Task<int> GetEntryCountAsync()
        {
            using (var context = new PlaylistContext())
                return await GetEntities(context).CountAsync().ConfigureAwait(false);
        }

        protected abstract DbSet<TModel> GetEntities(PlaylistContext context);

        protected virtual async Task SaveInternal(TModel item, PlaylistContext context)
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
                    await Update(item, context).ConfigureAwait(false);
            }

            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        protected virtual void Delete(TModel item, PlaylistContext context)
        {
            context.Set<TModel>().Remove(item);
        }

        protected virtual void Create(TModel item, PlaylistContext context)
        {
            item.CreatedBy = System.Security.Principal.WindowsIdentity.GetCurrent().User.Value;
            item.CreatedOn = DateTime.UtcNow;

            context.Set<TModel>().Add(item);
        }

        protected virtual async Task Update(TModel item, PlaylistContext context)
        {
            var entity = await GetEntities(context).FindAsync(item.Id).ConfigureAwait(false);

            if (entity == null)
                return;

            item.UpdatedOn = DateTime.UtcNow;
            item.UpdatedBy = System.Security.Principal.WindowsIdentity.GetCurrent().User.Value;


            context.Entry(entity).CurrentValues.SetValues(item);
        }

        protected virtual Task<TModel> GetByIdInternalAsync(TKeyDataType id, PlaylistContext context)
        {
            return GetEntities(context).FirstOrDefaultAsync(p => EqualityComparer<TKeyDataType>.Default.Equals(p.Id, id));
        }

        // override if you need to include foreignkeys
        protected virtual Task<List<TModel>> GetInternalAsync(PlaylistContext context)
        {
            return GetEntities(context).ToListAsync();
        }
    }
}