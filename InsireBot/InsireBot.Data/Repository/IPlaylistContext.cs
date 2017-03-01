using System.Data.Entity;

namespace Maple.Data
{
    public interface IPlaylistContext : IDbContext
    {
        DbSet<Playlist> Playlists { get; set; }
        DbSet<MediaItem> MediaItems { get; set; }
        DbSet<MediaPlayer> Mediaplayers { get; set; }
    }
}
