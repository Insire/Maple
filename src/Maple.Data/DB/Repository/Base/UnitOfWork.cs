using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Maple.Data
{
    public abstract class UnitOfWork : IDisposable
    {
        internal DbContext Context { get; }

        protected bool IsDiposed { get; private set; }

        protected UnitOfWork(DbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task SaveChanges()
        {
            return Context.SaveChangesAsync();
        }

        public Task SaveChanges(CancellationToken token)
        {
            return Context.SaveChangesAsync(token);
        }

        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!IsDiposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    Context.Dispose();
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.

                // Note disposing has been done.
                IsDiposed = true;
            }
        }
    }
}
