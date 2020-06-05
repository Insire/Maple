using System;

namespace Maple
{
    public sealed class CreatePlaylistViewModel
    {
        public CreatePlaylistViewModel(Playlist playlist)
        {
            Playlist = playlist ?? throw new ArgumentNullException(nameof(playlist));
        }

        public Playlist Playlist { get; }
    }
}
