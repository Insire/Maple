using Maple.Core;
using Maple.Data;
using Maple.Localization.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Maple
{
    public class MediaPlayers : ChangeTrackingViewModeListBase<MediaPlayer>, IDisposable
    {
        private readonly ITranslationManager _manager;
        private readonly IMediaPlayerRepository _mediaPlayerRepository;

        private readonly Playlists _playlists;

        private bool _disposed;
        public bool Disposed
        {
            get { return _disposed; }
            protected set { SetValue(ref _disposed, value); }
        }

        public MediaPlayers(ITranslationManager manager,
                            IMediaPlayerRepository mediaPlayerRepository,
                            Func<IMediaPlayer> playerFactory,
                            Playlists playlists)
        {
            _manager = manager;
            _mediaPlayerRepository = mediaPlayerRepository;

            _playlists = playlists;

            AddRange(GetMediaPlayers(playerFactory).ToList());
            SelectedItem = Items.FirstOrDefault();
        }

        private IEnumerable<MediaPlayer> GetMediaPlayers(Func<IMediaPlayer> playerFactory)
        {
            var all = _mediaPlayerRepository.GetAll();
            var players = all.Where(p => !p.IsPrimary)
                                .Select(p => new MediaPlayer(_manager, _mediaPlayerRepository, playerFactory(), p));

            var primaries = all.Where(p => p.IsPrimary).ToList();

            if ((primaries?.Count ?? 0) == 0)
            {
                var mediaPlayer = new Data.MediaPlayer
                {
                    Name = nameof(Resources.MainMediaplayer),
                    Sequence = 0,
                    IsPrimary = true,
                };

                var playlist = _playlists.Items.FirstOrDefault();

                yield return new MainMediaPlayer(_manager, _mediaPlayerRepository, playerFactory(), mediaPlayer, playlist, nameof(Resources.MainMediaplayer));
            }

            if (primaries.Count == 1)
            {
                var playlist = _playlists.Items.FirstOrDefault(p => p.Id == primaries[0].PlaylistId);
                yield return new MainMediaPlayer(_manager, _mediaPlayerRepository, playerFactory(), primaries[0], playlist, nameof(Resources.MainMediaplayer));
            }

            foreach (var player in players)
                yield return player;

            if (primaries?.Count > 1)
                throw new InsireBotException(Resources.InvalidMediaplayerCountOnDBException + $"({primaries.Count})");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                foreach (var player in Items)
                    player.Dispose();

                // Free any other managed objects here.
            }

            // Free any unmanaged objects here.
            Disposed = true;
        }
    }
}
