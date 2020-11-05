using System;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    public sealed class PlaybackViewModel : ObservableObject
    {
        public MediaPlayers MediaPlayers { get; }

        private MediaPlayer _selectedMediaPlayer;
        public MediaPlayer SelectedMediaPlayer
        {
            get { return _selectedMediaPlayer; }
            set { SetValue(ref _selectedMediaPlayer, value); }
        }

        public PlaybackViewModel(MediaPlayers mediaPlayers)
        {
            MediaPlayers = mediaPlayers ?? throw new ArgumentNullException(nameof(mediaPlayers));
        }
    }
}
