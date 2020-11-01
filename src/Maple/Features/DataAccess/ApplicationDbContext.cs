using Maple.Domain;
using Microsoft.EntityFrameworkCore;

namespace Maple
{
    public sealed class ApplicationDbContext : DbContext
    {
        public DbSet<PlaylistModel> Playlists { get; set; }
        public DbSet<MediaItemModel> MediaItems { get; set; }
        public DbSet<MediaPlayerModel> MediaPlayers { get; set; }

        public DbSet<AudioDeviceModel> AudioDevices { get; set; }
        public DbSet<AudioDeviceTypeModel> AudioDeviceTypes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new MediaPlayerConfiguration());
            modelBuilder.ApplyConfiguration(new MediaItemConfiguration());
            modelBuilder.ApplyConfiguration(new PlaylistConfiguration());
            modelBuilder.ApplyConfiguration(new AudioDeviceTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AudioDeviceConfiguration());
        }
    }
}
