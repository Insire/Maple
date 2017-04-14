using Maple.Core;
using Maple.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Maple
{
    // helpful: https://blog.oneunicorn.com/2012/03/10/secrets-of-detectchanges-part-1-what-does-detectchanges-do/
    /// <summary>
    /// Provides a way to access all playback related data on the DAL
    /// </summary>
    /// <seealso cref="Maple.IMediaRepository" />
    public class MediaRepository : IMediaRepository
    {
        private readonly PlaylistContext _context;
        private readonly AudioDevices _devices;
        private readonly BusyStack _busyStack;
        private readonly DialogViewModel _dialog;
        private readonly ITranslationService _manager;
        private readonly IMediaPlayer _mediaPlayer;

        private bool _disposed = false;

        public bool IsBusy { get; private set; }

        public MediaRepository(ITranslationService manager, IMediaPlayer mediaPlayer, DialogViewModel dialog, AudioDevices devices)
        {
            _manager = manager ?? throw new ArgumentNullException(nameof(manager));
            _mediaPlayer = mediaPlayer ?? throw new ArgumentNullException(nameof(mediaPlayer));
            _dialog = dialog ?? throw new ArgumentNullException(nameof(dialog));
            _devices = devices ?? throw new ArgumentNullException(nameof(devices));

            _context = new PlaylistContext();
            _busyStack = new BusyStack();
            _busyStack.OnChanged += (hasItems) => { IsBusy = hasItems; };
        }

        /// <summary>
        /// Gets the playlist by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Playlist GetPlaylistById(int id)
        {
            var playlist = _context.Playlists.FirstOrDefault(p => p.Id == id);

            if (playlist == null)
                return default(Playlist);

            var viewModel = new Playlist(_dialog, playlist);
            viewModel.AddRange(GetMediaItemByPlaylistId(playlist.Id));

            return viewModel;
        }

        public IList<Playlist> GetAllPlaylists()
        {
            var playlists = new List<Playlist>();
            var mediaItems = new List<MediaItem>();
            mediaItems.AddRange(_context.MediaItems.AsEnumerable().Select(p => new MediaItem(p)));
            playlists.AddRange(_context.Playlists.AsEnumerable().Select(p => new Playlist(_dialog, p)));

            foreach (var playlist in playlists)
                playlist.AddRange(mediaItems.Where(p => p.PlaylistId == playlist.Id));

            return playlists;
        }

        public void Save(Playlist playlist)
        {
            using (_busyStack.GetToken())
            {
                if (playlist.IsNew)
                    Create(playlist);
                else
                {
                    if (playlist.IsDeleted)
                        Delete(playlist);
                    else
                        Update(playlist);
                }

                _context.SaveChanges();
            }
        }

        private void Delete(Playlist playlist)
        {
            _context.Set<Data.Playlist>().Remove(playlist.Model);
        }

        private void Create(Playlist playlist)
        {
            _context.Set<Data.Playlist>().Add(playlist.Model);
        }

        private void Update(Playlist playlist)
        {
            var model = playlist.Model;
            var entitiy = _context.Playlists.Find(model.Id);

            entitiy.Description = model.Description;
            entitiy.IsShuffeling = model.IsShuffeling;
            entitiy.Location = model.Location;
            entitiy.PrivacyStatus = model.PrivacyStatus;
            entitiy.RepeatMode = model.RepeatMode;
            entitiy.Sequence = model.Sequence;
            entitiy.Title = model.Title;

            foreach (var item in playlist.Items)
                Save(item);
        }

        public void Save(Playlists playlists)
        {
            using (_busyStack.GetToken())
            {
                foreach (var playlist in playlists.Items)
                    Save(playlist);
            }
        }

        public MediaPlayer GetMainMediaPlayer()
        {
            var player = _context.Mediaplayers.FirstOrDefault(p => p.IsPrimary);

            if (player != null)
                return new MainMediaPlayer(_manager, _mediaPlayer, player, GetPlaylistById(player.PlaylistId), _devices);

            return default(MediaPlayer);
        }

        public MediaPlayer GetMediaPlayerById(int id)
        {
            var player = _context.Mediaplayers.FirstOrDefault(p => p.Id == id);

            if (player != null)
                return new MediaPlayer(_manager, _mediaPlayer, player, GetPlaylistById(player.PlaylistId), _devices);

            return default(MediaPlayer);
        }

        /// <summary>
        /// returns all non MainMediaPlayers
        /// </summary>
        /// <returns></returns>
        public IList<MediaPlayer> GetAllOptionalMediaPlayers()
        {
            var result = new List<MediaPlayer>();
            var players = _context.Mediaplayers
                                  .Where(p => !p.IsPrimary)
                                  .AsEnumerable()
                                  .Select(p => new MediaPlayer(_manager, _mediaPlayer, p, GetPlaylistById(p.PlaylistId), _devices));

            result.AddRange(players);

            return result;
        }

        public void Save(MediaPlayer player)
        {
            using (_busyStack.GetToken())
            {
                if (player.IsNew)
                    Create(player);
                else
                {
                    if (player.IsDeleted)
                        Delete(player);
                    else
                        Update(player);
                }

                _context.SaveChanges();
            }
        }

        private void Delete(MediaPlayer player)
        {
            _context.Set<Data.MediaPlayer>().Remove(player.Model);
        }

        private void Create(MediaPlayer player)
        {
            _context.Set<Data.MediaPlayer>().Add(player.Model);
        }

        private void Update(MediaPlayer player)
        {
            using (_busyStack.GetToken())
            {
                var model = player.Model;
                var entitiy = _context.Mediaplayers.Find(model.Id);

                entitiy.DeviceName = model.DeviceName;
                entitiy.IsPrimary = model.IsPrimary;
                entitiy.Name = model.Name;
                entitiy.PlaylistId = model.PlaylistId;
                entitiy.Sequence = model.Sequence;
            }
        }

        public void Save(MediaPlayers players)
        {
            using (_busyStack.GetToken())
            {
                foreach (var player in players.Items)
                    Save(player);
            }
        }

        private void Delete(MediaItem item)
        {
            _context.Set<Data.MediaItem>().Remove(item.Model);
        }

        private void Create(MediaItem item)
        {
            _context.Set<Data.MediaItem>().Add(item.Model);
        }

        private void Update(MediaItem item)
        {
            using (_busyStack.GetToken())
            {
                var model = item.Model;
                var entitiy = _context.MediaItems.Find(model.Id);

                entitiy.Location = model.Location;
                entitiy.Playlist = model.Playlist;
                entitiy.PrivacyStatus = model.PrivacyStatus;
                entitiy.PlaylistId = model.PlaylistId;
                entitiy.Title = model.Title;
                entitiy.Duration = model.Duration;
                entitiy.Description = model.Description;
                entitiy.Sequence = model.Sequence;
            }
        }

        public MediaItem GetMediaItemById(int id)
        {
            var item = _context.MediaItems.FirstOrDefault(p => p.Id == id);

            if (item != null)
                return new MediaItem(item);

            return default(MediaItem);
        }

        public IList<MediaItem> GetMediaItemByPlaylistId(int id)
        {
            var result = new List<MediaItem>();
            result.AddRange(_context.MediaItems.Where(p => p.PlaylistId == id).AsEnumerable().Select(p => new MediaItem(p)));

            return result;
        }

        public IList<MediaItem> GetAllMediaItems()
        {
            var result = new List<MediaItem>();
            result.AddRange(_context.MediaItems.AsEnumerable().Select(p => new MediaItem(p)));

            return result;
        }

        public void Save(MediaItem item)
        {
            using (_busyStack.GetToken())
            {
                if (item.IsNew)
                    Create(item);
                else
                {
                    if (item.IsDeleted)
                        Delete(item);
                    else
                        Update(item);
                }

                _context.SaveChanges();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _context.Dispose();
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            _disposed = true;
        }
    }
}
