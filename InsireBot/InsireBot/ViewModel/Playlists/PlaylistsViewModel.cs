using InsireBot.Core;
using InsireBot.Data;
using InsireBot.Localization.Properties;
using System.Linq;
using System.Windows.Input;

namespace InsireBot
{
    public class PlaylistsViewModel : ViewModelListBase<PlaylistViewModel>, ISaveable
    {
        private readonly IBotLog _log;
        private readonly IPlaylistsRepository _playlistRepository;
        private readonly IMediaItemRepository _mediaItemRepository;

        public ICommand SaveCommand { get; private set; }
        public ICommand AddCommand { get; private set; }
        public ICommand PlayCommand { get; private set; }

        public PlaylistsViewModel(IMediaItemRepository mediaItemRepository, IPlaylistsRepository playlistRepository, IBotLog log)
        {
            _mediaItemRepository = mediaItemRepository;
            _playlistRepository = playlistRepository;
            _log = log;

            var playlists = playlistRepository.GetAll();
            Items.AddRange(playlists.Select(p => new PlaylistViewModel(log, p)));

            SaveCommand = new RelayCommand<PlaylistViewModel>(Save, CanSave);
            AddCommand = new RelayCommand(Add, CanAdd);
        }

        // TODO order changing + sync, Commands, UserInteraction, Reset?, async load, save and delete

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

            Items.Add(new PlaylistViewModel(_log, playlist));
        }

        public void Save()
        {
            SaveAllInternal();
        }

        public void Save(PlaylistViewModel viewmodel)
        {
            if (viewmodel == null)
                SaveAllInternal();
            else
                SaveInternal(viewmodel);
        }

        public bool CanSave(PlaylistViewModel viewmodel)
        {
            if (viewmodel == null)
                return CanSaveAllInternal();

            return CanSaveInternal(viewmodel);
        }

        private void SaveInternal(PlaylistViewModel viewmodel)
        {
            _playlistRepository.Save(viewmodel.Model);
        }

        private bool CanSaveInternal(PlaylistViewModel viewmodel)
        {
            return viewmodel.IsValid && viewmodel.IsChanged;
        }

        private void SaveAllInternal()
        {
            var models = Items.Where(p => p.IsChanged)
                    .Select(p => p.Model)
                    .ToList();

            foreach (var model in models)
                _playlistRepository.Save(model);
        }

        private bool CanSaveAllInternal()
        {
            if (Items.Any(p => !p.IsValid))
                return false;

            return Items.Any(p => p.IsChanged);
        }
    }
}
