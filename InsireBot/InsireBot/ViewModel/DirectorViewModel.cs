using InsireBot.Core;

namespace InsireBot
{
    public class DirectorViewModel : ObservableObject
    {
        private MediaPlayers _mediaPlayers;
        public MediaPlayers MediaPlayers
        {
            get { return _mediaPlayers; }
            private set { SetValue(ref _mediaPlayers, value); }
        }

        private Playlists _playlists;
        public Playlists Playlists
        {
            get { return _playlists; }
            private set { SetValue(ref _playlists, value); }
        }

        public DirectorViewModel(MediaPlayers players, Playlists playlists)
        {
            Playlists = playlists;
            MediaPlayers = players;
        }
    }
}
