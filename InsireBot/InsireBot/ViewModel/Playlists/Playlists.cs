using Maple.Core;
using Maple.Data;
using Maple.Localization.Properties;
using System.Linq;
using System.Windows.Input;

namespace Maple
{
    public class Playlists : ViewModelListBase<Playlist>, ISaveable
    {
        private readonly IBotLog _log;
        private readonly IPlaylistsRepository _playlistRepository;
        private readonly IMediaItemRepository _mediaItemRepository;

        public ICommand SaveCommand { get; private set; }
        public ICommand AddCommand { get; private set; }
        public ICommand PlayCommand { get; private set; }

        public Playlists(IMediaItemRepository mediaItemRepository, IPlaylistsRepository playlistRepository, IBotLog log)
        {
            _mediaItemRepository = mediaItemRepository;
            _playlistRepository = playlistRepository;
            _log = log;

            var playlists = playlistRepository.GetAll();
            Items.AddRange(playlists.Select(p => new Playlist(log, p)));
            SelectedItem = Items.FirstOrDefault();

            SaveCommand = new RelayCommand<Playlist>(Save, CanSave);
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

            Items.Add(new Playlist(_log, playlist));
        }

        public void Save()
        {
            SaveAllInternal();
        }

        public void Save(Playlist viewmodel)
        {
            if (viewmodel == null)
                SaveAllInternal();
            else
                SaveInternal(viewmodel);
        }

        public bool CanSave(Playlist viewmodel)
        {
            if (viewmodel == null)
                return CanSaveAllInternal();

            return CanSaveInternal(viewmodel);
        }

        private void SaveInternal(Playlist viewmodel)
        {
            _playlistRepository.Save(viewmodel.Model);
            viewmodel.AcceptChanges();
        }

        private bool CanSaveInternal(Playlist viewmodel)
        {
            return viewmodel.IsValid && viewmodel.IsChanged;
        }

        private void SaveAllInternal()
        {
           Items.Where(p => p.IsChanged && p.IsValid)
                .ForEach(p => SaveInternal(p));
        }

        private bool CanSaveAllInternal()
        {
            if (Items.Any(p => !p.IsValid))
                return false;

            return Items.Any(p => p.IsChanged);
        }
    }
}
