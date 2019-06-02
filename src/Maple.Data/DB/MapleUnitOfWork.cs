using Maple.Domain;

namespace Maple.Data
{
    public sealed class MapleUnitOfWork : UnitOfWork, IUnitOfWork
    {
        public IMediaItemRepository MediaItems { get; }
        public IMediaPlayerRepository MediaPlayers { get; }
        public IPlaylistRepository Playlists { get; }

        public MapleUnitOfWork(PlaylistContext context)
            : base(context)
        {
            MediaItems = new MediaItemRepository(this);
            MediaPlayers = new MediaPlayerRepository(this);
            Playlists = new PlaylistRepository(this);
        }
    }
}
