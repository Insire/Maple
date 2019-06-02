using System;
using System.Threading.Tasks;

namespace Maple.Domain
{
    public interface IUnitOfWork
    {
        IMediaItemRepository MediaItems { get; }
        IMediaPlayerRepository MediaPlayers { get; }
        IPlaylistRepository Playlists { get; }

        Task SaveChanges();
    }
}
