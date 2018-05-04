using System;
using System.Linq;
using System.Threading.Tasks;

using Maple.Domain;

namespace Maple.Data
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private bool _disposed;

        private MapleContext _context;

        public UnitOfWork()
            : this(new MapleContext())
        {

        }

        public UnitOfWork(MapleContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public void Rollback()
        {
            _context
                .ChangeTracker
                .Entries()
                .ToList()
                .ForEach(x => x.Reload());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!_disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    if (_context != null)
                    {
                        _context.Dispose();
                        _context = null;
                    }
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.


                // Note disposing has been done.
                _disposed = true;
            }
        }
    }
}
