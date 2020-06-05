using Maple.Domain;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    public sealed class Playlists : ViewModelListBase<Playlist>
    {
        private readonly PlaylistFactory _playlistFactory;

        public Playlists(IMapleCommandBuilder commandBuilder, PlaylistFactory playlistFactory)
            : base(commandBuilder)
        {
            _playlistFactory = playlistFactory;
        }

        internal CreatePlaylistViewModel Create()
        {
            return _playlistFactory.Create();
        }

        internal CreatePlaylistViewModel Create(Playlist playlist)
        {
            return _playlistFactory.Create(playlist);
        }
    }
}
