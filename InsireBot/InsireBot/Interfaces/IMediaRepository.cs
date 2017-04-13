﻿using System;
using System.Collections.Generic;

namespace Maple
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IMediaRepository : IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether this instance is busy.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is busy; otherwise, <c>false</c>.
        /// </value>
        bool IsBusy { get; }

        /// <summary>
        /// Gets all media items.
        /// </summary>
        /// <returns></returns>
        List<MediaItem> GetAllMediaItems();
        /// <summary>
        /// Gets all optional media players.
        /// </summary>
        /// <returns></returns>
        List<MediaPlayer> GetAllOptionalMediaPlayers();
        /// <summary>
        /// Gets all playlists.
        /// </summary>
        /// <returns></returns>
        List<Playlist> GetAllPlaylists();

        /// <summary>
        /// Gets the media item by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        MediaItem GetMediaItemById(int id);
        /// <summary>
        /// Gets the media player by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        MediaPlayer GetMediaPlayerById(int id);
        /// <summary>
        /// Gets the playlist by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Playlist GetPlaylistById(int id);

        /// <summary>
        /// Gets the main media player.
        /// </summary>
        /// <returns></returns>
        MediaPlayer GetMainMediaPlayer();

        /// <summary>
        /// Saves the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        void Save(MediaItem item);
        /// <summary>
        /// Saves the specified player.
        /// </summary>
        /// <param name="player">The player.</param>
        void Save(MediaPlayer player);
        /// <summary>
        /// Saves the specified players.
        /// </summary>
        /// <param name="players">The players.</param>
        void Save(MediaPlayers players);
        /// <summary>
        /// Saves the specified playlist.
        /// </summary>
        /// <param name="playlist">The playlist.</param>
        void Save(Playlist playlist);
        /// <summary>
        /// Saves the specified playlists.
        /// </summary>
        /// <param name="playlists">The playlists.</param>
        void Save(Playlists playlists);
    }
}