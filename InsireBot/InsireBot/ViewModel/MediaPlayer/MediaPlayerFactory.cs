using Maple.Data;
using Maple.Localization.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Maple
{
    public static class MediaPlayerFactory
    {
        public static List<MediaPlayer> GetMediaPlayers(PlaylistContext context, ITranslationManager manager, Func<IMediaPlayer> playerFactory, Playlists playlists, AudioDevices devices, DialogViewModel dialog)
        {
            var result = new List<MediaPlayer>();
            var all = context.Mediaplayers.ToList();
            result.AddRange(all.Where(p => !p.IsPrimary)
                             .Select(p => new MediaPlayer(context, manager, playerFactory(), p, devices, dialog)));

            var primary = all.First(p => p.IsPrimary);
            var playlist = playlists.Items.FirstOrDefault(p => p.Id == primary.PlaylistId);
            result.Add(new MainMediaPlayer(context, manager, playerFactory(), primary, playlist, devices, nameof(Resources.MainMediaplayer)));

            return result;
        }
    }
}
