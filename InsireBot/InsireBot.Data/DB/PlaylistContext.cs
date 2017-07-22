using System.Data.Entity;

namespace Maple.Data
{
    public class PlaylistContext : DbContext
    {
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<MediaItem> MediaItems { get; set; }
        public DbSet<MediaPlayer> Mediaplayers { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Raw> Data { get; set; }

        public PlaylistContext()
            : base("Main")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ModelConfiguration.Configure(modelBuilder);
            Database.SetInitializer(new CreateSeedDatabaseIfNotExists<PlaylistContext>(modelBuilder));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
