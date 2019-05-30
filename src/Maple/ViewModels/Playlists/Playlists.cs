using System;
using System.Threading;
using System.Threading.Tasks;
using Maple.Core;
using Maple.Domain;

namespace Maple
{
    public sealed class Playlists : MapleBusinessViewModelListBase<PlaylistModel, Playlist>
    {
        public Playlists(IMapleCommandBuilder commandBuilder)
            : base(commandBuilder)
        {
        }

        protected override Task RefreshInternal(CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
