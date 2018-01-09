using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maple.Core
{
    public interface IMediaRepository : IDisposable
    {
        bool IsBusy { get; }

        Task SaveAsync(MediaItem viewModel);
        Task SaveAsync(MediaItems viewModel);
        Task<MediaItem> GetMediaItemByIdAsync(int id);
        Task<IList<MediaItem>> GetMediaItemsAsync();
        Task<MediaItem> GetMediaItemByPlaylistIdAsync(int id);

        Task SaveAsync(Playlist viewModel);
        Task SaveAsync(Playlists viewModel);
        Task<Playlist> GetPlaylistByIdAsync(int id);
        Task<IList<Playlist>> GetPlaylistsAsync();

        Task SaveAsync(MediaPlayer viewModel);
        Task SaveAsync(MediaPlayers viewModel);
        Task<MediaPlayer> GetMainMediaPlayerAsync();
        Task<MediaPlayer> GetMediaPlayerByIdAsync(int id);
        Task<IList<MediaPlayer>> GetAllOptionalMediaPlayersAsync();
    }
}