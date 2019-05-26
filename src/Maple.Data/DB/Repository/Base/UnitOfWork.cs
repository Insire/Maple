using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Maple.Data
{
    public abstract class UnitOfWork
    {
        internal DbContext Context { get; }

        protected UnitOfWork(DbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task SaveChanges()
        {
            return Context.SaveChangesAsync();
        }
    }
}
