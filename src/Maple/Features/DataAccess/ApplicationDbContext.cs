using Maple.Domain;
using Microsoft.EntityFrameworkCore;

namespace Maple
{
    public sealed class ApplicationDbContext : DbContext
    {
        public DbSet<PlaylistModel> Playlists { get; set; }
        public DbSet<MediaItemModel> MediaItems { get; set; }
        public DbSet<MediaPlayerModel> Mediaplayers { get; set; }

        public DbSet<AudioDeviceModel> AudioDevices { get; set; }

        public DbSet<AudioDeviceTypeModel> AudioDeviceTypes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
