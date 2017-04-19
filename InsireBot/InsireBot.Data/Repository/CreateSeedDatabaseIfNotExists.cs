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
                       Title = "Main Playlist",
                       Description = "Test playlist with 3 entries",
                       Id = 1,
                       IsShuffeling = false,
                       Location = "",
                       PrivacyStatus = 0,
                       RepeatMode = 1,
                       Sequence = 0,
                   });

            if (context.Playlists.Find(2) == null)
                context.Playlists
                   .Add(new Playlist
                   {
                       Title = "Another Playlist with a very long title",
                       Description = "Playlist for testing with 1 entry",
                       Id = 2,
                       IsShuffeling = true,
                       Location = "",
                       PrivacyStatus = 0,
                       RepeatMode = 0,
                       Sequence = 10,
                   });
        }

        private void SeedMediaItems(TContext context)
        {
            if (context.MediaItems.Find(1) == null)
                context.MediaItems
                        .Add(new MediaItem
                        {
                            Title = "Test Song",
                            Description = "A popular youtube video",
                            Duration = 60_000_000,
                            Id = 1,
                            Location = "https://www.youtube.com/watch?v=oHg5SJYRHA0",
                            PlaylistId = 1,
                            PrivacyStatus = 0,
                            Sequence = 0,
                        });

            if (context.MediaItems.Find(2) == null)
                context.MediaItems
                        .Add(new MediaItem
                        {
                            Title = "Incorrect Title",
                            Description = "A music video on youtube",
                            Duration = 60_000_000,
                            Id = 2,
                            Location = "https://www.youtube.com/watch?v=PXf4rkguwDI&t",
                            PlaylistId = 1,
                            PrivacyStatus = 0,
                            Sequence = 0,
                        });

            if (context.MediaItems.Find(3) == null)
                context.MediaItems
                        .Add(new MediaItem
                        {
                            Title = "Another Incorrect Title",
                            Description = "A music video on youtube",
                            Duration = 60_000_000,
                            Id = 3,
                            Location = "https://www.youtube.com/watch?v=xYfn7MWU7TQ&t",
                            PlaylistId = 1,
                            PrivacyStatus = 0,
                            Sequence = 0,
                        });

            if (context.MediaItems.Find(4) == null)
                context.MediaItems
                        .Add(new MediaItem
                        {
                            Title = "Another Incorrect Title",
                            Description = "A music video on youtube",
                            Duration = 60_000_000,
                            Id = 4,
                            Location = "https://www.youtube.com/watch?v=WS9ludtQmqs&t",
                            PlaylistId = 2,
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

            // 4

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
