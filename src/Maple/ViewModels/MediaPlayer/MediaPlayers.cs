using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Maple.Core;
using Maple.Domain;

namespace Maple
{
    public sealed class MediaPlayers : MapleDomainViewModelListBase<MediaPlayer>
    {
        public Playlists Playlists { get; }

        public MediaPlayers(IMapleCommandBuilder commandBuilder, IValidator<MediaPlayer> validator, Playlists playlists)
            : base(commandBuilder, validator)
        {
            Playlists = playlists ?? throw new ArgumentNullException(nameof(playlists));
        }

        protected override Task RefreshInternal(CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
