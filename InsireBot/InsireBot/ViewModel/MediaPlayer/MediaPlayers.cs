using Maple.Core;
using Maple.Data;
using Maple.Localization.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Maple
{
    public class MediaPlayers : ViewModelListBase<MediaPlayer>, IDisposable, ISaveable
    {
        private readonly IBotLog _log;
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
                            IBotLog log,
                            IMediaPlayerRepository mediaPlayerRepository,
                            Func<IMediaPlayer> playerFactory,
                            Playlists playlists)
        {
            _log = log;
            _manager = manager;
            _mediaPlayerRepository = mediaPlayerRepository;

            _playlists = playlists;

            Items.AddRange(GetMediaPlayers(playerFactory).ToList());
            SelectedItem = Items.FirstOrDefault();
        }

        private IEnumerable<MediaPlayer> GetMediaPlayers(Func<IMediaPlayer> playerFactory)
        {
            var all = _mediaPlayerRepository.GetAll();
            var players = all.Where(p => !p.IsPrimary)
                                .Select(p => new MediaPlayer(playerFactory(), p));

            var primaries = all.Where(p => p.IsPrimary).ToList();

            if ((primaries?.Count ?? 0) == 0)
            {
                var mediaPlayer = new Data.MediaPlayer
                {
                    Name = nameof(Resources.MainMediaplayer),
                    Sequence = 0,
                    IsPrimary = true,
                };

                yield return new MainMediaPlayer(_manager, playerFactory(), mediaPlayer, nameof(Resources.MainMediaplayer))
                {
                    Playlist = _playlists.Items.FirstOrDefault(),
                };
            }

            if (primaries.Count == 1)
                yield return new MainMediaPlayer(_manager, playerFactory(), primaries[0], nameof(Resources.MainMediaplayer))
                {
                    Playlist = _playlists.Items.FirstOrDefault(p => p.Id == primaries[0].PlaylistId),
                };


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

        public void Save()
        {
            var main = Items.First(p => p.IsPrimary);
            if (main.IsChanged && main.IsValid)
            {
                _mediaPlayerRepository.Save(main.Model);
                main.AcceptChanges();
            }

            var others = Items.Where(p => p.IsValid && p.IsChanged && !p.IsPrimary)
                                     .Select(p => p.Model);

            foreach (var player in others)
                _mediaPlayerRepository.Save(player);
        }
    }
}
