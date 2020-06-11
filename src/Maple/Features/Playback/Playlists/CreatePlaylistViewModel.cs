using System;

namespace Maple
{
    public sealed class CreatePlaylistViewModel
    {
        public Playlists Playlists { get; }
        public Playlist Playlist { get; }

        public CreatePlaylistViewModel(Playlists playlists, Playlist playlist)
        {
            Playlists = playlists ?? throw new ArgumentNullException(nameof(playlists));
            Playlist = playlist ?? throw new ArgumentNullException(nameof(playlist));
        }
    }
}
