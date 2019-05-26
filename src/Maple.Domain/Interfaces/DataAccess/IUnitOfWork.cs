using System;
using System.Threading.Tasks;

namespace Maple.Domain
{
    public interface IUnitOfWork
    {
        IMediaItemRepository MediaItemRepository { get; }
        IMediaPlayerRepository MediaPlayerRepository { get; }
        IPlaylistRepository PlaylistRepository { get; }

        Task SaveChanges();
    }
}
