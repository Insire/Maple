using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maple
{
    public interface IMediaRepository : IDisposable
    {
        bool IsBusy { get; }

        void Save(MediaItem viewModel);
        void Save(MediaItems viewModel);
        Task<MediaItem> GetMediaItemByIdAsync(int id);
        Task<IList<MediaItem>> GetMediaItemsAsync();
        Task<MediaItem> GetMediaItemByPlaylistIdAsync(int id);


        void Save(Playlist viewModel);
        void Save(Playlists viewModel);
        Task<Playlist> GetPlaylistByIdAsync(int id);
        Task<IList<Playlist>> GetPlaylistsAsync();

        void Save(MediaPlayer viewModel);
        void Save(MediaPlayers viewModel);
        Task<MediaPlayer> GetMainMediaPlayerAsync();
        Task<MediaPlayer> GetMediaPlayerByIdAsync(int id);
        Task<IList<MediaPlayer>> GetAllOptionalMediaPlayersAsync();
    }
}