using Maple.Domain;

namespace Maple.Data
{
    public sealed class MapleUnitOfWork : UnitOfWork, IUnitOfWork
    {
        public IMediaItemRepository MediaItemRepository { get; }
        public IMediaPlayerRepository MediaPlayerRepository { get; }
        public IPlaylistRepository PlaylistRepository { get; }

        public MapleUnitOfWork(PlaylistContext context)
            : base(context)
        {
            MediaItemRepository = new MediaItemRepository(this);
            MediaPlayerRepository = new MediaPlayerRepository(this);
            PlaylistRepository = new PlaylistRepository(this);
        }
    }
}
