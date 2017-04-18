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

        private void SeedOptions(TContext context)
        {
            if (context.Options.Find(1) == null)
                context.Options
                    .Add(new Option
                    {
                        Id = 1,
                        Key = "SelectedPlaylist",
                        Sequence = 0,
                        Type = (int)OptionType.Playlist,
                        Value = "1",
                    });

            if (context.Options.Find(2) == null)
                context.Options
                    .Add(new Option
                    {
                        Id = 2,
                        Key = "SelectedMediaItem",
                        Sequence = 10,
                        Type = (int)OptionType.MediaItem,
                        Value = "",
                    });

            if (context.Options.Find(3) == null)
                context.Options
                    .Add(new Option
                    {
                        Id = 3,
                        Key = "SelectedMediaPlayer",
                        Sequence = 20,
                        Type = (int)OptionType.MediaPlayer,
                        Value = "1",
                    });

            if (context.Options.Find(4) == null)
                context.Options
                    .Add(new Option
                    {
                        Id = 4,
                        Key = "SelectedCulture",
                        Sequence = 30,
                        Type = (int)OptionType.Culture,
                        Value = "0",
                    });

            if (context.Options.Find(5) == null)
                context.Options
                    .Add(new Option
                    {
                        Id = 5,
                        Key = "SelectedPrimary",
                        Sequence = 40,
                        Type = (int)OptionType.ColorProfile,
                        Value = "",
                    });

            if (context.Options.Find(6) == null)
                context.Options
                    .Add(new Option
                    {
                        Id = 6,
                        Key = "SelectedAccent",
                        Sequence = 50,
                        Type = (int)OptionType.ColorProfile,
                        Value = "",
                    });

            if (context.Options.Find(7) == null)
                context.Options
                    .Add(new Option
                    {
                        Id = 7,
                        Key = "SelectedScene",
                        Sequence = 60,
                        Type = (int)OptionType.Scene,
                        Value = "",
                    });

        }
    }
}
