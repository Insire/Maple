using System;
using Maple.Domain;
using MvvmScarletToolkit;

namespace Maple
{
    public sealed class PlaylistFactory
    {
        private readonly IScarletCommandBuilder _commandBuilder;

        public PlaylistFactory(IScarletCommandBuilder commandBuilder)
        {
            _commandBuilder = commandBuilder ?? throw new ArgumentNullException(nameof(commandBuilder));
        }

        public Playlist Create(PlaylistModel model)
        {
            return new Playlist(_commandBuilder, model);
        }

        public CreatePlaylistViewModel Create()
        {
            var playerViewModel = new Playlist(new Playlist(_commandBuilder));

            return Create(playerViewModel);
        }

        public CreatePlaylistViewModel Create(Playlist playlist)
        {
            return new CreatePlaylistViewModel(playlist);
        }
    }
}
