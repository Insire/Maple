using System.Data.Entity;

using Maple.Domain;

namespace Maple.Data
{
    public sealed class MapleContext : DbContext
    {
        public DbSet<PlaylistModel> Playlists { get; set; }
        public DbSet<MediaItemModel> MediaItems { get; set; }
        public DbSet<MediaPlayerModel> Mediaplayers { get; set; }

        public MapleContext()
            : base("Main")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ModelConfiguration.Configure(modelBuilder);
            Database.SetInitializer(new CreateSeedDatabaseIfNotExists(modelBuilder));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
