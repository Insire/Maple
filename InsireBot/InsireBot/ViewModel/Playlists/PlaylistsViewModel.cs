using InsireBot.Core;
using InsireBot.Data;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace InsireBot
{
    public class PlaylistsViewModel : ObservableObject
    {
        private readonly IBotLog _log;
        private readonly IPlaylistsRepository _playlistRepository;
        private readonly IMediaItemRepository _mediaItemRepository;

        public ChangeTrackingCollection<PlaylistViewModel> Items { get; private set; }

        private PlaylistViewModel _currentItem;
        public PlaylistViewModel CurrentItem
        {
            get { return _currentItem; }
            private set { SetValue(ref _currentItem, value); }
        }

        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public ICommand PlayCommand { get; private set; }

        public PlaylistsViewModel(IMediaItemRepository mediaItemRepository, IPlaylistsRepository playlistRepository, IBotLog log)
        {
            _mediaItemRepository = mediaItemRepository;
            _playlistRepository = playlistRepository;
            _log = log;

            var playlists = playlistRepository.GetAll();
            Items = new ChangeTrackingCollection<PlaylistViewModel>(playlists.Select(p => new PlaylistViewModel(log, p)));
            //var data = new Data.Playlist
            //{
            //    Description = "Test",
            //    Sequence = 0,
            //    Title = "Test",
            //    MediaItems = new List<Data.MediaItem>(),
            //};
            //var playlist = new PlaylistViewModel(log, data);

            //Items.Add(playlist);
            //Save(playlist);

            SaveCommand = new RelayCommand<PlaylistViewModel>(Save, CanSave);
            DeleteCommand = new RelayCommand<PlaylistViewModel>(Delete, CanDelete);
        }

        // TODO order changing + sync, Commands, UserInteraction, Reset?, async load, save and delete

        /// <summary>
        /// Sets the <see cref="MediaItemViewModel"/> as the <seealso cref="CurrentItem"/>
        /// Adds the <see cref="MediaItemViewModel"/> to the <seealso cref="History"/>
        /// </summary>
        /// <param name="playlist"></param>
        public virtual void SetActive(PlaylistViewModel playlist)
        {
            if (playlist != null)
            {
                CurrentItem = playlist;
            }
        }

        private bool CanSetActive(PlaylistViewModel item)
        {
            return false;
        }

        public void Delete(PlaylistViewModel item)
        {
            if (item == null)
                DeleteAllInternal();
            else
                DeleteInternal(item);
        }

        public bool CanDelete(PlaylistViewModel item)
        {
            if (item == null)
                return CanDeleteAllInternal();

            return CanDeleteInternal(item);
        }

        private void DeleteInternal(PlaylistViewModel playlist)
        {
            foreach (var item in playlist.Items.Select(p => p.Model))
                _mediaItemRepository.Delete(item);

            _playlistRepository.Delete(playlist.Model);
        }

        private bool CanDeleteInternal(PlaylistViewModel item)
        {
            return false;
        }

        private void DeleteAllInternal()
        {
            foreach (var playlist in Items)
                DeleteInternal(playlist);
        }

        private bool CanDeleteAllInternal()
        {
            return false;
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
