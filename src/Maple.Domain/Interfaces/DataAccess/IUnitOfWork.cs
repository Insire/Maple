using System;
using System.Threading;
using System.Threading.Tasks;

namespace Maple.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IMediaItemRepository MediaItems { get; }
        IMediaPlayerRepository MediaPlayers { get; }
        IPlaylistRepository Playlists { get; }

        Task Migrate(CancellationToken token);

        Task Migrate();

        Task SaveChanges();

        Task SaveChanges(CancellationToken token);
    }
}
