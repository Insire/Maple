using Maple.Data;
using Maple.Localization.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Maple
{
    public static class MediaPlayerFactory
    {
        public static IEnumerable<MediaPlayer> GetMediaPlayers(IPlaylistContext context, ITranslationManager manager, Func<IMediaPlayer> playerFactory, Playlists playlists)
        {
            var all = context.Mediaplayers.ToList();
            var players = all.Where(p => !p.IsPrimary)
                                .Select(p => new MediaPlayer(manager, playerFactory(), p));

            var primaries = all.Where(p => p.IsPrimary).ToList();

            if ((primaries?.Count ?? 0) == 0)
            {
                var mediaPlayer = new Data.MediaPlayer
                {
                    Name = nameof(Resources.MainMediaplayer),
                    Sequence = 0,
                    IsPrimary = true,
                };

                var playlist = playlists.Items.FirstOrDefault();

                yield return new MainMediaPlayer(manager, playerFactory(), mediaPlayer, playlist, nameof(Resources.MainMediaplayer));
            }

            if (primaries.Count == 1)
            {
                var playlist = playlists.Items.FirstOrDefault(p => p.Id == primaries[0].PlaylistId);
                yield return new MainMediaPlayer(manager, playerFactory(), primaries[0], playlist, nameof(Resources.MainMediaplayer));
            }

            foreach (var player in players)
                yield return player;

            if (primaries?.Count > 1)
                throw new InsireBotException(Resources.InvalidMediaplayerCountOnDBException + $"({primaries.Count})");
        }
    }
}
