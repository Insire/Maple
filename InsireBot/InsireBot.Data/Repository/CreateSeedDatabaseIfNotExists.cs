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

            context.Mediaplayers
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
