using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Maple.Core;
using Maple.Domain;

namespace Maple
{
    public sealed class MediaPlayers : MapleDomainViewModelListBase<MediaPlayers, MediaPlayer>
    {
        public Playlists Playlists { get; }

        public MediaPlayers(IMapleCommandBuilder commandBuilder, IValidator<MediaPlayers> validator, Playlists playlists)
            : base(commandBuilder, validator)
        {
            Playlists = playlists ?? throw new ArgumentNullException(nameof(playlists));
        }

        protected override Task RefreshInternal(CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}
