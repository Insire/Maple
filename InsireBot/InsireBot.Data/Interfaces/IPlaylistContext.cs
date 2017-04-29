using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Maple.Data
{
    public interface IPlaylistContext : IDisposable
    {
        DbSet<MediaItem> MediaItems { get; set; }
        DbSet<MediaPlayer> Mediaplayers { get; set; }
        DbSet<Option> Options { get; set; }
        DbSet<Playlist> Playlists { get; set; }
        DbSet<Raw> Data { get; set; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        int SaveChanges();
    }
}