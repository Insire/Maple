//using System;
//using System.Collections.Generic;
//using Maple.Domain;

//namespace Maple.Data
//{
//    public class CreateSeedDatabaseIfNotExists<TContext>
//    {
//        private const int saveThresHold = 100;

//        private readonly List<string> _playlistTitles = new List<string>()
//        {
//            "Memories Of A Time To Come",
//            "A Twist In The Myth",
//            "At The Edge Of Time",
//            "Beyond The Red Mirror",
//            "Battalions Of Fear",
//            "Follow The Blind",
//            "Tales From The Twilight World",
//            "Somewhere Far Beyond",
//            "Tokyo Tales",
//            "Imaginations From The Other Side",
//            "The Forgotten Tales",
//            "Nightfall In Middle Earth",
//            "A Night At The Opera",
//            "Infinite",
//            "The Slim Shady LP",
//            "Marshall Mathers",
//            "The Eminem Show",
//            "Encore",
//            "Relapse",
//            "Recovery",
//        };

//        private readonly List<string> _mediaItemTitles = new List<string>()
//        {
//            "The Ninth Wave",
//            "Twilight Of The Gods",
//            "Prophecies",
//            "At The Edge Of Time",
//            "Ashes Of Eternity",
//            "Distant Memories",
//            "The Holy Grail",
//            "The Throne",
//            "Sacred Mind",
//            "Miracle Machine",
//            "Grand Parade",
//            "My Name Is",
//            "Guilty Conscience",
//            "Brain Damage",
//            "Paul",
//            "If I Had",
//            "'97 Bonnie & Clyde",
//            "Bitch",
//            "Role Model",
//        };



//        //protected  void Seed(TContext context)
//        //{
//        //    if (!Debugger.IsAttached)
//        //    {
//        //        base.Seed(context);
//        //        return;
//        //    }

//        //    base.Seed(context);

//        //    SeedTestData(context);
//        //    SeedMediaPlayers(context);

//        //    context.SaveChanges();
//        //}

//        private void SeedTestData(TContext context)
//        {
//            context.Playlists
//                       .Add(new PlaylistModel
//                       {
//                           Title = "MP3 Files",
//                           Description = "Test playlist with mp3 files",
//                           Id = 0,
//                           IsShuffeling = false,
//                           Location = "https://www.youtube.com/watch?v=WxfcsmbBd00&t=0s",
//                           PrivacyStatus = 0,
//                           RepeatMode = 1,
//                           Sequence = 0,
//                           CreatedBy = "SYSTEM",
//                           UpdatedBy = "SYSTEM",
//                           CreatedOn = DateTime.UtcNow,
//                           UpdatedOn = DateTime.UtcNow,
//                       });

//            context.MediaItems
//                        .Add(new MediaItemModel
//                        {
//                            Title = "Universe Words",
//                            Description = "http://freemusicarchive.org/music/Artofescapism/",
//                            Duration = 60_000_000,
//                            Id = 0,
//                            Location = ".\\Resources\\Art_Of_Escapism_-_Universe_Words.mp3",
//                            Playlist = context.Playlists.Find(0),
//                            PrivacyStatus = 0,
//                            Sequence = 0,
//                            MediaItemType = (int)MediaItemType.LocalFile,
//                            CreatedBy = "SYSTEM",
//                            UpdatedBy = "SYSTEM",
//                            CreatedOn = DateTime.UtcNow,
//                            UpdatedOn = DateTime.UtcNow,
//                        });

//            var index = 0;
//            for (var i = 1; i < _playlistTitles.Count; i++)
//            {
//                context.Playlists
//                           .Add(new PlaylistModel
//                           {
//                               Title = _playlistTitles[i],
//                               Description = "Test playlist with 3 entries",
//                               Id = i,
//                               IsShuffeling = false,
//                               Location = "https://www.youtube.com/watch?v=WxfcsmbBd00&t=0s",
//                               PrivacyStatus = 0,
//                               RepeatMode = 1,
//                               Sequence = i,
//                               CreatedBy = "SYSTEM",
//                               UpdatedBy = "SYSTEM",
//                               CreatedOn = DateTime.UtcNow,
//                               UpdatedOn = DateTime.UtcNow,
//                           });

//                for (var j = 1; j < _mediaItemTitles.Count; j++)
//                {
//                    context.MediaItems
//                            .Add(new MediaItemModel
//                            {
//                                Title = _mediaItemTitles[j],
//                                Description = "A popular youtube video",
//                                Duration = 60_000_000,
//                                Id = j,
//                                Location = "https://www.youtube.com/watch?v=oHg5SJYRHA0",
//                                Playlist = context.Playlists.Find(i),
//                                PrivacyStatus = 0,
//                                Sequence = j,
//                                CreatedBy = "SYSTEM",
//                                UpdatedBy = "SYSTEM",
//                                CreatedOn = DateTime.UtcNow,
//                                UpdatedOn = DateTime.UtcNow,
//                            });
//                    index++;

//                    //if (index % saveThresHold == 0)
//                    //context.SaveChanges();
//                }
//            }

//        }

//        private void SeedMediaPlayers(TContext context)
//        {
//            if (context.Mediaplayers.Find(1) == null)
//                context.Mediaplayers
//                       .Add(new MediaPlayerModel
//                       {
//                           Id = 1,
//                           IsPrimary = true,
//                           Name = "Main",
//                           Playlist = context.Playlists.Find(1),
//                           Sequence = 0,
//                           CreatedBy = "SYSTEM",
//                           UpdatedBy = "SYSTEM",
//                           CreatedOn = DateTime.UtcNow,
//                           UpdatedOn = DateTime.UtcNow,
//                       });
//        }

//        private void SeedOptions(TContext context)
//        {
//            if (context.Options.Find(1) == null)
//                context.Options
//                    .Add(new OptionModel
//                    {
//                        Id = 1,
//                        Key = "SelectedPlaylist",
//                        Sequence = 0,
//                        Type = (int)OptionType.Playlist,
//                        Value = "1",
//                        CreatedBy = "SYSTEM",
//                        UpdatedBy = "SYSTEM",
//                        CreatedOn = DateTime.UtcNow,
//                        UpdatedOn = DateTime.UtcNow,
//                    });

//            if (context.Options.Find(2) == null)
//                context.Options
//                    .Add(new OptionModel
//                    {
//                        Id = 2,
//                        Key = "SelectedMediaItem",
//                        Sequence = 10,
//                        Type = (int)OptionType.MediaItem,
//                        Value = "",
//                        CreatedBy = "SYSTEM",
//                        UpdatedBy = "SYSTEM",
//                        CreatedOn = DateTime.UtcNow,
//                        UpdatedOn = DateTime.UtcNow,
//                    });

//            if (context.Options.Find(3) == null)
//                context.Options
//                    .Add(new OptionModel
//                    {
//                        Id = 3,
//                        Key = "SelectedMediaPlayer",
//                        Sequence = 20,
//                        Type = (int)OptionType.MediaPlayer,
//                        Value = "1",
//                        CreatedBy = "SYSTEM",
//                        UpdatedBy = "SYSTEM",
//                        CreatedOn = DateTime.UtcNow,
//                        UpdatedOn = DateTime.UtcNow,
//                    });

//            // 4

//            if (context.Options.Find(5) == null)
//                context.Options
//                    .Add(new OptionModel
//                    {
//                        Id = 5,
//                        Key = "SelectedPrimary",
//                        Sequence = 40,
//                        Type = (int)OptionType.ColorProfile,
//                        Value = "",
//                        CreatedBy = "SYSTEM",
//                        UpdatedBy = "SYSTEM",
//                        CreatedOn = DateTime.UtcNow,
//                        UpdatedOn = DateTime.UtcNow,
//                    });

//            if (context.Options.Find(6) == null)
//                context.Options
//                    .Add(new OptionModel
//                    {
//                        Id = 6,
//                        Key = "SelectedAccent",
//                        Sequence = 50,
//                        Type = (int)OptionType.ColorProfile,
//                        Value = "",
//                        CreatedBy = "SYSTEM",
//                        UpdatedBy = "SYSTEM",
//                        CreatedOn = DateTime.UtcNow,
//                        UpdatedOn = DateTime.UtcNow,
//                    });

//            if (context.Options.Find(7) == null)
//                context.Options
//                    .Add(new OptionModel
//                    {
//                        Id = 7,
//                        Key = "SelectedScene",
//                        Sequence = 60,
//                        Type = (int)OptionType.Scene,
//                        Value = "",
//                        CreatedBy = "SYSTEM",
//                        UpdatedBy = "SYSTEM",
//                        CreatedOn = DateTime.UtcNow,
//                        UpdatedOn = DateTime.UtcNow,
//                    });
//        }
//    }
//}
