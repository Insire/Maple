using System;
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
