using Maple.Core;
using Maple.Data;
using System;

namespace Maple
{
    public class MediaPlayers : BaseListViewModel<MediaPlayer>, IDisposable
    {
        private bool _disposed;
        public bool Disposed
        {
            get { return _disposed; }
            protected set { SetValue(ref _disposed, value); }
        }

        public MediaPlayers(PlaylistContext context, ITranslationManager manager, Func<IMediaPlayer> playerFactory, Playlists playlists, AudioDevices devices, DialogViewModel dialog)
        {
            Items.AddRange(MediaPlayerFactory.GetMediaPlayers(context, manager, playerFactory, playlists, devices, dialog));
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
