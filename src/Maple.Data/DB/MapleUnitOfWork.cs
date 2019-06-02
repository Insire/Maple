using System.Threading;
using System.Threading.Tasks;
using Maple.Domain;

namespace Maple.Data
{
    public sealed class MapleUnitOfWork : UnitOfWork<PlaylistContext>, IUnitOfWork
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

        public Task Migrate(CancellationToken token)
        {
            return Context.Migrate(token);
        }

        public Task Migrate()
        {
            return Context.Migrate(CancellationToken.None);
        }
    }
}
