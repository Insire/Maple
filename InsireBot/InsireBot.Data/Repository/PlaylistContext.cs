using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Maple.Data
{
    public class PlaylistContext : LoggingContext
    {
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<MediaItem> MediaItems { get; set; }
        public DbSet<MediaPlayer> Mediaplayers { get; set; }

        public PlaylistContext() : base("Main")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new CreateSeedDatabaseIfNotExists<PlaylistContext>(modelBuilder));
        }
    }


    public class CreateSeedDatabaseIfNotExists<TContext> : SqliteDropCreateDatabaseWhenModelChanges<TContext> where TContext : PlaylistContext
    {
        public CreateSeedDatabaseIfNotExists(DbModelBuilder builder)
            : base(builder)
        {

        }

        protected override void Seed(TContext context)
        {
            base.Seed(context);

            context.Set<Playlist>()
                   .Add(new Playlist
                   {
                       Description = "Base",
                       Id = 1,
                       IsShuffeling = false,
                       Location = "",
                       PrivacyStatus = 0,
                       MediaItems = new List<MediaItem>(),
                       RepeatMode = 0,
                       Sequence = 0,
                       Title = "Base",
                   });

            context.Set<MediaPlayer>()
                   .Add(new MediaPlayer
                   {
                       Id = 1,
                       IsPrimary = true,
                       Name = "Main",
                       PlaylistId = 1,
                       Sequence = 0,
                   });
        }
    }

    public class LoggingContext : DbContext
    {
        public EventHandler Saving;
        public EventHandler Saved;

        public LoggingContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public override int SaveChanges()
        {
            Saving?.Invoke(this, EventArgs.Empty);
            var result = base.SaveChanges();
            Saved?.Invoke(this, EventArgs.Empty);

            return result;
        }
    }
}
