using System;
using System.Threading.Tasks;

namespace Maple.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();

        Task CommitAsync();

        void Rollback();
    }
}
