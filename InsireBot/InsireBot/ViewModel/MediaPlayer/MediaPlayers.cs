using Maple.Core;
using Maple.Data;
using Maple.Localization.Properties;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows.Input;

namespace Maple
{
    public class MediaPlayers : BaseDataListViewModel<MediaPlayer, Data.MediaPlayer>, IDisposable, ILoadableViewModel, ISaveableViewModel
    {
        private readonly ITranslationManager _manager;
        private readonly Func<IMediaPlayer> _playerFactory;
        private readonly AudioDevices _devices;
        private readonly DialogViewModel _dialog;

        private bool _disposed;
        public bool Disposed
        {
            get { return _disposed; }
            protected set { SetValue(ref _disposed, value); }
        }

        public ICommand LoadCommand => new RelayCommand(Load, () => !IsLoaded);
        public ICommand RefreshCommand => new RelayCommand(Load);
        public ICommand SaveCommand => new RelayCommand(Save);

        public bool IsLoaded { get; private set; }

        public MediaPlayers(ITranslationManager manager, Func<IMediaPlayer> playerFactory, AudioDevices devices, DialogViewModel dialog)
        {
            _manager = manager;
            _playerFactory = playerFactory;
            _devices = devices;
            _dialog = dialog;
        }

        public void Load()
        {
            Items.Clear();

            var tuple = GetContexts();
            var primary = tuple.MediaPlayers.FirstOrDefault(p => p.IsPrimary);
            var secondary = tuple.MediaPlayers.Where(p => !p.IsPrimary)
                                              .Select(p => new MediaPlayer(_manager, _playerFactory(), p, GetPlaylistViewModel(p), _devices));

            if (primary != null)
            {
                Items.Add(new MainMediaPlayer(_manager, _playerFactory(), primary, GetPlaylistViewModel(primary), _devices, nameof(Resources.MainMediaplayer)));
                SelectedItem = Items[0];
            }

            Items.AddRange(secondary);
            if (SelectedItem == null && Items.Count > 0)
                SelectedItem = Items[0];

            IsLoaded = true;

            (List<Data.MediaPlayer> MediaPlayers, List<Data.Playlist> Playlists) GetContexts()
            {
                using (var context = new PlaylistContext())
                    return (MediaPlayers: context.Mediaplayers.ToList(), Playlists: context.Playlists.ToList());
            }

            Playlist GetPlaylistViewModel(Data.MediaPlayer player)
            {
                return new Playlist(_dialog, GetPlaylist(player));
            }

            Data.Playlist GetPlaylist(Data.MediaPlayer model)
            {
                return tuple.Playlists.FirstOrDefault(p => p.Id == model.PlaylistId);
            }
        }

        public void Save()
        {
            using (var context = new PlaylistContext())
            {
                foreach (var player in Items)
                {
                    context.Entry(player.Model).State = player.Model.Id == 0
                                                            ? EntityState.Added
                                                            : EntityState.Modified;
                }

                context.SaveChanges();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                foreach (var player in Items)
                    player.Dispose();

                // Free any other managed objects here.
            }

            // Free any unmanaged objects here.
            Disposed = true;
        }
    }
}
