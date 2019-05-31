using System.Threading.Tasks;
using Maple.Domain;
using Microsoft.EntityFrameworkCore;

namespace Maple.Data
{
    public sealed class PlaylistContext : DbContext
    {
        public DbSet<PlaylistModel> Playlists { get; set; }
        public DbSet<MediaItemModel> MediaItems { get; set; }
        public DbSet<MediaPlayerModel> Mediaplayers { get; set; }
        public DbSet<OptionModel> Options { get; set; }

        public PlaylistContext(DbContextOptions<PlaylistContext> options)
            : base(options)
        {
        }

        public Task Migrate()
        {
            return Database.MigrateAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MediaPlayerConfiguration());
            modelBuilder.ApplyConfiguration(new MediaItemConfiguration());
            modelBuilder.ApplyConfiguration(new PlaylistConfiguration());
            modelBuilder.ApplyConfiguration(new OptionConfiguration());
        }
    }
}
