using SQLite.CodeFirst;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;

namespace Maple.Data
{
    public class PlaylistContext : DbContext
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

            modelBuilder.Entity<Playlist>()
                .HasOptional(a => a.MediaItems)
                .WithOptionalDependent()
                .WillCascadeOnDelete(true);
        }
    }

    internal class CreateSeedDatabaseIfNotExists<TContext> : SqliteDropCreateDatabaseWhenModelChanges<TContext> where TContext : PlaylistContext
    {
        public CreateSeedDatabaseIfNotExists(DbModelBuilder builder)
            : base(builder)
        {
        }

        protected override void Seed(TContext context)
        {
            base.Seed(context);

            if (!Debugger.IsAttached)
                return;

            context.Set<Playlist>()
                   .Add(new Playlist
                   {
                       Description = "Base",
                       Id = 1,
                       IsShuffeling = false,
                       Location = "",
                       PrivacyStatus = 0,
                       MediaItems = new List<MediaItem>
                       {
                           new MediaItem
                           {
                               Title = "Test",
                               Description = "Description",
                               Duration = 60_000,
                               Id= 1,
                               Location = "C:",
                               PlaylistId = 1,
                               PrivacyStatus=0,
                               Sequence=0,
                           },
                       },
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

            context.SaveChanges();
        }
    }
}
