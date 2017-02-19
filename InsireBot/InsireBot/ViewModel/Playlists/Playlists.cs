using Maple.Core;
using Maple.Data;
using Maple.Localization.Properties;
using System.Linq;
using System.Windows.Input;

namespace Maple
{
    public class Playlists : ChangeTrackingViewModeListBase<Playlist>
    {
        private readonly IBotLog _log;
        private readonly IPlaylistsRepository _playlistRepository;
        private readonly IMediaItemRepository _mediaItemRepository;
        private readonly DialogViewModel _dialogViewModel;

        public ICommand PlayCommand { get; private set; }

        public Playlists(IMediaItemRepository mediaItemRepository, IPlaylistsRepository playlistRepository, IBotLog log, DialogViewModel dialogViewModel)
        {
            _mediaItemRepository = mediaItemRepository;
            _playlistRepository = playlistRepository;
            _dialogViewModel = dialogViewModel;
            _log = log;

            var playlists = playlistRepository.Seed();
            Items.AddRange(playlists.Select(p => new Playlist(_playlistRepository, _mediaItemRepository, _dialogViewModel, p)));
            SelectedItem = Items.FirstOrDefault();

            AddCommand = new RelayCommand(Add, CanAdd);
        }

        // TODO order changing + sync, Commands, UserInteraction, async load

        public void Add()
        {
            var playlist = Data.Playlist.New();

            if (Count > 0)
            {
                var index = 0;
                var current = Items.Select(p => p.Sequence).ToList();
                while (current.IndexOf(index) >= 0)
                {
                    if (index == int.MaxValue)
                    {
                        _log.Error(Resources.MaxPlaylistCountReachedException);
                        return;
                    }

                    index++;
                }
                playlist.Sequence = index;
            }

            Items.Add(new Playlist(_playlistRepository, _mediaItemRepository, _dialogViewModel, playlist));
        }
    }
}
