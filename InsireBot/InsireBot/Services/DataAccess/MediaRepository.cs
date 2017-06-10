using Maple.Core;
using Maple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maple
{
    // helpful: https://blog.oneunicorn.com/2012/03/10/secrets-of-detectchanges-part-1-what-does-detectchanges-do/
    /// <summary>
    /// Provides a way to access all playback related data on the DAL
    /// </summary>
    /// <seealso cref="Maple.IMediaRepository" />
    public class MediaRepository : IMediaRepository
    {
        private const int _saveThreshold = 100;

        private readonly IPlaylistContext _context;

        private readonly IPlaylistMapper _playlistMapper;
        private readonly IMediaPlayerMapper _mediaPlayerMapper;
        private readonly IMediaItemMapper _mediaItemMapper;

        private readonly BusyStack _busyStack;

        private bool _disposed = false;

        public bool IsBusy { get; private set; }

        public MediaRepository(IPlaylistMapper playlistMapper, IMediaItemMapper mediaItemMapper, IMediaPlayerMapper mediaPlayerMapper, IPlaylistContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            _mediaItemMapper = mediaItemMapper ?? throw new ArgumentNullException(nameof(mediaItemMapper));
            _playlistMapper = playlistMapper ?? throw new ArgumentNullException(nameof(playlistMapper));
            _mediaPlayerMapper = mediaPlayerMapper ?? throw new ArgumentNullException(nameof(mediaPlayerMapper));

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

            var viewModel = _playlistMapper.Get(playlist);
            viewModel.AddRange(GetMediaItemByPlaylistId(playlist.Id));

            return viewModel;
        }

        public async Task<Playlist> GetPlaylistByIdAsync(int id)
        {
            var playlist = await Task.Run(() => _context.Playlists.FirstOrDefault(p => p.Id == id));

            if (playlist == null)
                return default(Playlist);

            var viewModel = _playlistMapper.Get(playlist);
            viewModel.AddRange(GetMediaItemByPlaylistId(playlist.Id));

            return viewModel;
        }

        public IList<Playlist> GetAllPlaylists()
        {
            var playlists = _context.Playlists.AsEnumerable().Select(p => _playlistMapper.Get(p)).ToList();
            var mediaItems = _context.MediaItems.AsEnumerable().Select(p => _mediaItemMapper.Get(p)).ToList();

            foreach (var playlist in playlists)
                playlist.AddRange(mediaItems.Where(p => p.PlaylistId == playlist.Id));

            return playlists;
        }

        public async Task<IList<Playlist>> GetAllPlaylistsAsync()
        {
            var data = await Task.Run(() => _context.Playlists.ToList());
            var playlists = data.Select(p => _playlistMapper.Get(p));
            var mediaItems = await GetAllMediaItemsAsync();

            foreach (var playlist in playlists)
                playlist.AddRange(mediaItems.Where(p => p.PlaylistId == playlist.Id));

            return playlists.ToList();
        }

        public void Save(Playlist playlist, bool isRoot = true)
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

                if (isRoot)
                    _context.SaveChanges();
            }
        }

        private void Delete(Playlist playlist)
        {
            _context.Set<Data.Playlist>().Remove(playlist.Model);
        }

        private void Create(Playlist playlist)
        {
            playlist.Model.CreatedBy = System.Security.Principal.WindowsIdentity.GetCurrent().User.Value;
            playlist.Model.CreatedOn = DateTime.UtcNow;

            _context.Set<Data.Playlist>().Add(playlist.Model);

            foreach (var item in playlist.Items)
                Save(item, false);
        }

        private void Update(Playlist playlist)
        {
            var model = playlist.Model;
            var entity = _context.Playlists.Find(model.Id);

            if (entity == null)
                return;

            playlist.UpdatedOn = DateTime.UtcNow;
            playlist.UpdatedBy = System.Security.Principal.WindowsIdentity.GetCurrent().User.Value;

            _context.Entry(entity).CurrentValues.SetValues(playlist.Model);

            foreach (var item in playlist.Items)
                Save(item, false);
        }

        public void Save(Playlists playlists, bool isRoot = true)
        {
            using (_busyStack.GetToken())
            {
                var saveRequired = false;
                for (var i = 0; i < playlists.Items.Count; i++)
                {
                    saveRequired = true;
                    Save(playlists.Items[i], false);

                    if (i % _saveThreshold == 0)
                    {
                        _context.SaveChanges();
                        saveRequired = false;
                    }

                }

                if (isRoot || saveRequired)
                    _context.SaveChanges();
            }
        }

        public MediaPlayer GetMainMediaPlayer()
        {
            using (_busyStack.GetToken())
            {
                var player = _context.Mediaplayers.FirstOrDefault(p => p.IsPrimary);

                if (player != null)
                    return _mediaPlayerMapper.Get(player, GetPlaylistById(player.PlaylistId));

                return default(MediaPlayer);
            }
        }

        public async Task<MediaPlayer> GetMainMediaPlayerAsync()
        {
            using (_busyStack.GetToken())
            {
                var player = await Task.Run(() => _context.Mediaplayers.FirstOrDefault(p => p.IsPrimary));

                if (player != null)
                {
                    var playlist = await GetPlaylistByIdAsync(player.PlaylistId);

                    if (playlist != null)
                        return _mediaPlayerMapper.GetMain(player, GetPlaylistById(player.PlaylistId));
                }

                return default(MediaPlayer);
            }
        }

        public MediaPlayer GetMediaPlayerById(int id)
        {
            using (_busyStack.GetToken())
            {
                var player = _context.Mediaplayers.FirstOrDefault(p => p.Id == id);

                if (player != null)
                    return _mediaPlayerMapper.Get(player, GetPlaylistById(player.PlaylistId));

                return default(MediaPlayer);
            }
        }

        public async Task<MediaPlayer> GetMediaPlayerByIdAsync(int id)
        {
            using (_busyStack.GetToken())
            {
                var player = await Task.Run(() => _context.Mediaplayers.FirstOrDefault(p => p.Id == id));

                if (player != null)
                {
                    var playlist = await GetPlaylistByIdAsync(player.PlaylistId);

                    if (playlist != null)
                        return _mediaPlayerMapper.Get(player, GetPlaylistById(player.PlaylistId));
                }

                return default(MediaPlayer);
            }
        }

        /// <summary>
        /// returns all non MainMediaPlayers
        /// </summary>
        /// <returns></returns>
        public IList<MediaPlayer> GetAllOptionalMediaPlayers()
        {
            using (_busyStack.GetToken())
            {
                var result = new List<MediaPlayer>();
                var players = _context.Mediaplayers
                                      .Where(p => !p.IsPrimary)
                                      .AsEnumerable()
                                      .Select(p => _mediaPlayerMapper.Get(p, GetPlaylistById(p.PlaylistId)));

                result.AddRange(players);

                return result;
            }
        }

        public async Task<IList<MediaPlayer>> GetAllOptionalMediaPlayersAsync()
        {
            using (_busyStack.GetToken())
            {
                var result = new List<MediaPlayer>();
                var players = await Task.Run(() => _context.Mediaplayers.Where(p => !p.IsPrimary));

                foreach (var player in players)
                {
                    var playlist = await GetPlaylistByIdAsync(player.PlaylistId);
                    result.Add(_mediaPlayerMapper.Get(player, GetPlaylistById(player.PlaylistId)));
                }

                return result;
            }
        }

        public void Save(MediaPlayer player, bool isRoot = true)
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

                if (isRoot)
                    _context.SaveChanges();
            }
        }

        private void Delete(MediaPlayer player)
        {
            _context.Set<Data.MediaPlayer>().Remove(player.Model);
        }

        private void Create(MediaPlayer player)
        {
            player.Model.CreatedBy = System.Security.Principal.WindowsIdentity.GetCurrent().User.Value;
            player.Model.CreatedOn = DateTime.UtcNow;

            _context.Set<Data.MediaPlayer>().Add(player.Model);
        }

        private void Update(MediaPlayer player)
        {
            using (_busyStack.GetToken())
            {
                var model = player.Model;
                var entity = _context.Mediaplayers.Find(model.Id);

                if (entity == null)
                    return;

                player.Model.UpdatedOn = DateTime.UtcNow;
                player.Model.UpdatedBy = System.Security.Principal.WindowsIdentity.GetCurrent().User.Value;

                _context.Entry(entity).CurrentValues.SetValues(player.Model);
            }
        }

        public void Save(IMediaPlayersViewModel players, bool isRoot = true)
        {
            using (_busyStack.GetToken())
            {
                var saveRequired = false;
                for (var i = 0; i < players.Items.Count; i++)
                {
                    saveRequired = true;
                    Save(players.Items[i], false);

                    if (i % _saveThreshold == 0)
                    {
                        _context.SaveChanges();
                        saveRequired = false;
                    }
                }

                if (isRoot || saveRequired)
                    _context.SaveChanges();
            }
        }

        private void Delete(MediaItem item)
        {
            _context.Set<Data.MediaItem>().Remove(item.Model);
        }

        private void Create(MediaItem item)
        {
            item.Model.CreatedBy = System.Security.Principal.WindowsIdentity.GetCurrent().User.Value;
            item.Model.CreatedOn = DateTime.UtcNow;

            _context.Set<Data.MediaItem>().Add(item.Model);
        }

        private void Update(MediaItem item)
        {
            using (_busyStack.GetToken())
            {
                var model = item.Model;
                var entity = _context.MediaItems.Find(model.Id);

                if (entity == null)
                    return;

                entity.UpdatedOn = DateTime.UtcNow;
                entity.UpdatedBy = System.Security.Principal.WindowsIdentity.GetCurrent().User.Value;

                _context.Entry(entity).CurrentValues.SetValues(item.Model);
            }
        }

        public MediaItem GetMediaItemById(int id)
        {
            using (_busyStack.GetToken())
            {
                var item = _context.MediaItems.FirstOrDefault(p => p.Id == id);

                if (item != null)
                    return _mediaItemMapper.Get(item);

                return default(MediaItem);
            }
        }

        public async Task<MediaItem> GetMediaItemByIdAsync(int id)
        {
            using (_busyStack.GetToken())
            {
                var item = await Task.Run(() => _context.MediaItems.FirstOrDefault(p => p.Id == id));

                if (item != null)
                    return _mediaItemMapper.Get(item);

                return default(MediaItem);
            }
        }

        public IList<MediaItem> GetMediaItemByPlaylistId(int id)
        {
            using (_busyStack.GetToken())
            {
                return _context.MediaItems.Where(p => p.PlaylistId == id)
                                          .AsEnumerable()
                                          .Select(p => _mediaItemMapper.Get(p))
                                          .ToList();
            }
        }

        public async Task<IList<MediaItem>> GetMediaItemByPlaylistIdAsync(int id)
        {
            using (_busyStack.GetToken())
            {
                var items = await Task.Run(() => _context.MediaItems.Where(p => p.PlaylistId == id).AsEnumerable());

                return _mediaItemMapper.GetManyAsList(items);
            }
        }

        public IList<MediaItem> GetAllMediaItems()
        {
            using (_busyStack.GetToken())
            {
                var items = _context.MediaItems.AsEnumerable();

                return _mediaItemMapper.GetManyAsList(items);
            }
        }

        public async Task<IList<MediaItem>> GetAllMediaItemsAsync()
        {
            using (_busyStack.GetToken())
            {
                var items = await Task.Run(() => _context.MediaItems.AsEnumerable());

                return _mediaItemMapper.GetManyAsList(items);
            }
        }

        public void Save(MediaItem item, bool isRoot = true)
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

                if (isRoot)
                    _context.SaveChanges();
            }
        }

        public void Save(IMediaItemsViewModel mediaItems, bool isRoot = true)
        {
            using (_busyStack.GetToken())
            {
                var saveRequired = false;
                for (var i = 0; i < mediaItems.Items.Count; i++)
                {
                    saveRequired = true;
                    Save(mediaItems.Items[i], false);

                    if (i % _saveThreshold == 0)
                    {
                        _context.SaveChanges();
                        saveRequired = false;
                    }
                }

                if (isRoot || saveRequired)
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
            }

            // Free any unmanaged objects here.
            _disposed = true;
        }
    }
}
