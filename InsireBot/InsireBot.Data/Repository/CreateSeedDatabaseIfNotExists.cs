using SQLite.CodeFirst;
using System.Data.Entity;
using System.Diagnostics;

namespace Maple.Data
{
    internal class CreateSeedDatabaseIfNotExists<TContext> : SqliteDropCreateDatabaseWhenModelChanges<TContext>
        where TContext : PlaylistContext
    {
        public CreateSeedDatabaseIfNotExists(DbModelBuilder builder)
            : base(builder)
        {
        }

        protected override void Seed(TContext context)
        {
            if (!Debugger.IsAttached)
            {
                base.Seed(context);
                return;
            }

            base.Seed(context);

            SeedPlaylists(context);
            SeedMediaItems(context);
            SeedMediaPlayers(context);

            context.SaveChanges();
        }

        private void SeedPlaylists(TContext context)
        {
            if (context.Playlists.Find(1) == null)
                context.Playlists
                   .Add(new Playlist
                   {
                       Description = "Base",
                       Id = 1,
                       IsShuffeling = false,
                       Location = "",
                       PrivacyStatus = 0,
                       RepeatMode = 0,
                       Sequence = 0,
                       Title = "Base",
                   });
        }

        private void SeedMediaItems(TContext context)
        {
            if (context.MediaItems.Find(1) == null)
                context.MediaItems
                        .Add(new MediaItem
                        {
                            Title = "Test",
                            Description = "Description",
                            Duration = 60_000,
                            Id = 1,
                            Location = "C:",
                            PlaylistId = 1,
                            PrivacyStatus = 0,
                            Sequence = 0,
                        });
        }

        private void SeedMediaPlayers(TContext context)
        {
            if (context.Mediaplayers.Find(1) == null)
                context.Mediaplayers
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
}
