using Maple.Domain;
using Microsoft.Extensions.Logging;

namespace Maple.Data
{
    public sealed class MapleUnitOfWork : UnitOfWork, IUnitOfWork
    {
        private readonly bool _shouldDispose;

        public IMediaItemRepository MediaItemRepository { get; }
        public IMediaPlayerRepository MediaPlayerRepository { get; }
        public IPlaylistRepository PlaylistRepository { get; }

        public MapleUnitOfWork(ILoggerFactory loggerFactory)
            : this(new PlaylistContext(loggerFactory))
        {
            _shouldDispose = true;
        }

        internal MapleUnitOfWork(PlaylistContext context)
            : base(context)
        {
            MediaItemRepository = new MediaItemRepository(this);
            MediaPlayerRepository = new MediaPlayerRepository(this);
            PlaylistRepository = new PlaylistRepository(this);
        }

        public override void Dispose()
        {
            if (_shouldDispose)
                Context.Dispose();
        }
    }
}
