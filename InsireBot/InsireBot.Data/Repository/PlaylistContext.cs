using System.Data.Entity;

namespace Maple.Data
{
    public class PlaylistContext : DbContext
    {
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<MediaItem> MediaItems { get; set; }
        public DbSet<MediaPlayer> Mediaplayers { get; set; }

        public PlaylistContext()
            : base("Main")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new CreateSeedDatabaseIfNotExists<PlaylistContext>(modelBuilder));

            //modelBuilder.Entity<MediaItem>()
            //    .HasRequired(p => p.Playlist)
            //    .WithMany(p => p.MediaItems)
            //    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<MediaPlayer>()
            //    .HasRequired(p => p.Playlist);
        }
    }
}
