using Maple.Core;
using Maple.Data;
using Maple.Localization.Properties;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Maple
{
    public class MediaPlayers : BaseDataListViewModel<MediaPlayer, Data.MediaPlayer>, IDisposable, IRefreshable
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

        public MediaPlayers(ITranslationManager manager, Func<IMediaPlayer> playerFactory, AudioDevices devices, DialogViewModel dialog)
        {
            _manager = manager;
            _playerFactory = playerFactory;
            _devices = devices;
            _dialog = dialog;
        }

        public Task LoadAsync()
        {
            return Task.Run(() =>
            {
                Items.Clear();

                using (var context = new PlaylistContext())
                {
                    var mediaPlayers = context.Mediaplayers.ToList();
                    var primary = mediaPlayers.FirstOrDefault(p => p.IsPrimary);
                    var secondary = mediaPlayers.Where(p => !p.IsPrimary)
                                                .Select(p => new MediaPlayer(_manager, _playerFactory(), p, GetPlaylistViewModel(p), _devices));
                    if (primary != null)
                    {
                        Items.Add(new MainMediaPlayer(_manager, _playerFactory(), primary, GetPlaylistViewModel(primary), _devices, nameof(Resources.MainMediaplayer)));
                        SelectedItem = Items[0];
                    }

                    Items.AddRange(secondary);
                    if (SelectedItem == null && Items.Count > 0)
                        SelectedItem = Items[0];

                    Playlist GetPlaylistViewModel(Data.MediaPlayer player)
                    {
                        return new Playlist(_dialog, GetPlaylist(player));
                    }

                    Data.Playlist GetPlaylist(Data.MediaPlayer model)
                    {
                        return context.Playlists.FirstOrDefault(p => p.Id == model.PlaylistId);
                    }
                }
            });
        }

        public Task SaveAsync()
        {
            return Task.Run(() =>
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
            });
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
