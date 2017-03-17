using System;
using System.Collections.Generic;

namespace Maple
{
    public interface IMediaRepository : IDisposable
    {
        bool IsBusy { get; }

        List<MediaItem> GetAllMediaItems();
        List<MediaPlayer> GetAllOptionalMediaPlayers();
        List<Playlist> GetAllPlaylists();

        MediaItem GetMediaItemById(int id);
        MediaPlayer GetMediaPlayerById(int id);
        Playlist GetPlaylistById(int id);

        MediaPlayer GetMainMediaPlayer();

        void Save(MediaItem item);
        void Save(MediaPlayer player);
        void Save(MediaPlayers players);
        void Save(Playlist playlist);
        void Save(Playlists playlists);
    }
}