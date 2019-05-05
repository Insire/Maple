using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Maple.Domain;
using Maple.Log;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Maple.Data
{
    public sealed class PlaylistContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        public DbSet<PlaylistModel> Playlists { get; set; }
        public DbSet<MediaItemModel> MediaItems { get; set; }
        public DbSet<MediaPlayerModel> Mediaplayers { get; set; }
        public DbSet<OptionModel> Options { get; set; }

        public PlaylistContext(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        internal PlaylistContext(DbContextOptions<PlaylistContext> options, ILoggerFactory loggerFactory)
            : base(options)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public Task Migrate()
        {
            return Database.MigrateAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;

            _loggerFactory.AddLog4Net();

            optionsBuilder
                .EnableSensitiveDataLogging(true)
                .UseLoggerFactory(_loggerFactory)
                .UseSqlite(Constants.ConnectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
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
