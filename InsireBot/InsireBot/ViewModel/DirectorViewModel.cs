using InsireBot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InsireBot
{
    public class DirectorViewModel : ObservableObject
    {
        private IBotLog _log;
        public ChangeTrackingCollection<MediaPlayerViewModel> MediaPlayers { get; private set; }
        public PlaylistsViewModel Playlists { get; private set; }

        public ICommand AddCommand { get; private set; }
        public ICommand RemoveCommand { get; private set; }

        public DirectorViewModel(IBotLog log, IMediaPlayer player, PlaylistsViewModel playlists)
        {
            _log = log;
            Playlists = playlists;

            MediaPlayers = new ChangeTrackingCollection<MediaPlayerViewModel>();
            MediaPlayers.Add(new MainMediaPlayerViewModel(player));
            // host a bunch of mediaplayers
            // main for music
            // follower
            // (re)subscriber
            // donation
            // host
        }
    }
}
