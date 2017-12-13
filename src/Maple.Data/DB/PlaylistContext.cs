using System.Data.Entity;
using Maple.Domain;

namespace Maple.Data
{
    public class PlaylistContext : DbContext
    {
        public DbSet<PlaylistModel> Playlists { get; set; }
        public DbSet<MediaItemModel> MediaItems { get; set; }
        public DbSet<MediaPlayerModel> Mediaplayers { get; set; }
        public DbSet<OptionModel> Options { get; set; }
        public DbSet<RawModel> Data { get; set; }

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
