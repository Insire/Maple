using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Maple.Domain;

namespace Maple
{
    public sealed class MediaPlayers : MapleDomainViewModelListBase<MediaPlayers, MediaPlayer>
    {
        private readonly AudioDevices _audioDevices;
        private readonly IValidator<MediaPlayer> _mediaPlayerValidator;

        public Playlists Playlists { get; }

        public MediaPlayers(IMapleCommandBuilder commandBuilder, IValidator<MediaPlayers> validator, IValidator<MediaPlayer> mediaPlayerValidator, Playlists playlists, AudioDevices audioDevices)
            : base(commandBuilder, validator)
        {
            Playlists = playlists ?? throw new ArgumentNullException(nameof(playlists));
            _mediaPlayerValidator = mediaPlayerValidator ?? throw new ArgumentNullException(nameof(mediaPlayerValidator));
            _audioDevices = audioDevices ?? throw new ArgumentNullException(nameof(audioDevices));
        }

        protected override async Task RefreshInternal(CancellationToken token)
        {
            using (var context = ContextFactory())
            {
                var players = await context.MediaPlayers
                     .ReadAsync(token)
                     .ConfigureAwait(false);

                await Task.WhenAll(players.ForEach(Add)).ConfigureAwait(false);
            }
        }

        private Task Add(MediaPlayerModel model)
        {
            return Add(new MediaPlayer((IMapleCommandBuilder)CommandBuilder, new MaplePlayer((IMapleCommandBuilder)CommandBuilder), _mediaPlayerValidator, _audioDevices, Playlists, null, model));
        }
    }
}
