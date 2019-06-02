using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Maple.Domain;

namespace Maple
{
    public sealed class Playlists : MapleBusinessViewModelListBase<PlaylistModel, Playlist>
    {
        private readonly IValidator<Playlist> _playlistValidator;
        private readonly IValidator<MediaItem> _mediaitemValidator;
        private readonly DialogViewModel _dialogViewModel;

        public Playlists(IMapleCommandBuilder commandBuilder, IValidator<Playlist> playlistValidator, IValidator<MediaItem> mediaitemValidator, DialogViewModel dialogViewModel)
            : base(commandBuilder)
        {
            _playlistValidator = playlistValidator ?? throw new ArgumentNullException(nameof(playlistValidator));
            _mediaitemValidator = mediaitemValidator ?? throw new ArgumentNullException(nameof(mediaitemValidator));
            _dialogViewModel = dialogViewModel ?? throw new ArgumentNullException(nameof(dialogViewModel));
        }

        protected override async Task RefreshInternal(CancellationToken token)
        {
            using (var context = ContextFactory())
            {
                var playlists = await context.Playlists
                     .ReadAsync(token)
                     .ConfigureAwait(false);

                await Task.WhenAll(playlists.ForEach(Add)).ConfigureAwait(false);
            }
        }

        private Task Add(PlaylistModel model)
        {
            return Add(new Playlist((IMapleCommandBuilder)CommandBuilder, _playlistValidator, _mediaitemValidator, _dialogViewModel, model));
        }
    }
}
