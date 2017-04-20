using SQLite.CodeFirst;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;

namespace Maple.Data
{
    public class CreateSeedDatabaseIfNotExists<TContext> : SqliteDropCreateDatabaseWhenModelChanges<TContext>
        where TContext : PlaylistContext
    {
        private const int saveThresHold = 100;

        private readonly List<string> _playlistTitles = new List<string>()
        {
            "Memories Of A Time To Come",
            "A Twist In The Myth",
            "At The Edge Of Time",
            "Beyond The Red Mirror",
            "Battalions Of Fear",
            "Follow The Blind",
            "Tales From The Twilight World",
            "Somewhere Far Beyond",
            "Tokyo Tales",
            "Imaginations From The Other Side",
            "The Forgotten Tales",
            "Nightfall In Middle Earth",
            "A Night At The Opera",

            "Infinite",
            "The Slim Shady LP",
            "Marshall Mathers",
            "The Eminem Show",
            "Encore",
            "Relapse",
            "Recovery",
        };

        private readonly List<string> _mediaItemTitles = new List<string>()
        {
            "The Ninth Wave",
            "Twilight Of The Gods",
            "Prophecies",
            "At The Edge Of Time",
            "Ashes Of Eternity",
            "Distant Memories",
            "The Holy Grail",
            "The Throne",
            "Sacred Mind",
            "Miracle Machine",
            "Grand Parade",

            "My Name Is",
            "Guilty Conscience",
            "Brain Damage",
            "Paul",
            "If I Had",
            "'97 Bonnie & Clyde",
            "Bitch",
            "Role Model",
        };

        public CreateSeedDatabaseIfNotExists(DbModelBuilder builder)
            : base(builder)
        {
        }

        public CreateSeedDatabaseIfNotExists(DbModelBuilder modelBuilder, System.Type historyEntityType)
            : base(modelBuilder, historyEntityType)
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

            SeedTestData(context);
            SeedMediaPlayers(context);

            context.SaveChanges();
        }

        private void SeedTestData(TContext context)
        {
            var index = 0;
            for (var i = 0; i < _playlistTitles.Count; i++)
            {
                context.Playlists
                       .Add(new Playlist
                       {
                           Title = _playlistTitles[i],
                           Description = "Test playlist with 3 entries",
                           Id = i,
                           IsShuffeling = false,
                           Location = "https://www.youtube.com/watch?v=WxfcsmbBd00&t=0s",
                           PrivacyStatus = 0,
                           RepeatMode = 1,
                           Sequence = i,
                       });

                for (var j = 0; j < _mediaItemTitles.Count; j++)
                {
                    context.MediaItems
                            .Add(new MediaItem
                            {
                                Title = _mediaItemTitles[j],
                                Description = "A popular youtube video",
                                Duration = 60_000_000,
                                Id = j,
                                Location = "https://www.youtube.com/watch?v=oHg5SJYRHA0",
                                PlaylistId = i,
                                PrivacyStatus = 0,
                                Sequence = j,
                            });
                    index++;

                    if (index % saveThresHold == 0)
                        context.SaveChanges();
                }
            }

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
