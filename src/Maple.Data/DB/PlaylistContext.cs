using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Maple.Domain;
using Microsoft.Data.Sqlite;
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

        public async Task Migrate(CancellationToken token)
        {
            try
            {
                await Database
                    .MigrateAsync(token)
                    .ConfigureAwait(false);
            }
            catch (SqliteException ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
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
