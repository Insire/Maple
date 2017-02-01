using InsireBot.Core;
using InsireBot.Data;
using InsireBot.Localization.Properties;
using System;
using System.Linq;
using System.Windows.Input;

namespace InsireBot
{
    public class DirectorViewModel : ObservableObject, ISaveable
    {
        private readonly IBotLog _log;
        private readonly ITranslationManager _manager;
        private readonly IMediaPlayerRepository _mediaPlayerRepository;
        public ChangeTrackingCollection<MediaPlayerViewModel> MediaPlayers { get; private set; }

        private MediaPlayerViewModel _selectedItem;
        public MediaPlayerViewModel SelectedItem
        {
            get { return _selectedItem; }
            set { SetValue(ref _selectedItem, value); }
        }

        public PlaylistsViewModel Playlists { get; private set; }

        public ICommand AddCommand { get; private set; }
        public ICommand RemoveCommand { get; private set; }

        public DirectorViewModel(ITranslationManager manager, IBotLog log, IMediaPlayerRepository mediaPlayerRepository, Func<IMediaPlayer> playerFactory, PlaylistsViewModel playlists)
        {
            _log = log;
            _manager = manager;
            _mediaPlayerRepository = mediaPlayerRepository;
            Playlists = playlists;

            MediaPlayers = new ChangeTrackingCollection<MediaPlayerViewModel>();
            MediaPlayers.Add(InitializeMainMediaPlayerViewModel(playerFactory));

            // host a bunch of mediaplayers
            // main for music
            // follower
            // (re)subscriber
            // donation
            // host
        }

        public void Save()
        {
            Playlists.Save();

            // TODO
            //foreach(var player in MediaPlayers)
            //    player.
        }

        private MainMediaPlayerViewModel InitializeMainMediaPlayerViewModel(Func<IMediaPlayer> playerFactory)
        {
            var primaries = _mediaPlayerRepository.GetPrimary().ToList();
            var player = playerFactory();

            if ((primaries?.Count ?? 0) == 0)
            {
                return new MainMediaPlayerViewModel(_manager, player, nameof(Resources.MainMediaplayer))
                {
                    Playlist = Playlists.Items.FirstOrDefault(),
                };
            }

            if (primaries.Count == 1)
            {
                return new MainMediaPlayerViewModel(_manager, player, nameof(Resources.MainMediaplayer))
                {
                    Playlist = Playlists.Items.FirstOrDefault(p=>p.ID == primaries[0].PlaylistId),
                };
            }

            throw new InsireBotException(Resources.InvalidMediaplayerCountOnDBException + $"({primaries.Count })");
        }
    }
}
