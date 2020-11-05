using System;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    public sealed class CreateMediaPlayerViewModel : ObservableObject
    {
        public AudioDevices AudioDevices { get; }
        public Playlists Playlists { get; }

        public MediaPlayer MediaPlayer { get; }

        public CreateMediaPlayerViewModel(AudioDevices audioDevices, Playlists playlists, MediaPlayer mediaPlayer)
        {
            AudioDevices = audioDevices ?? throw new ArgumentNullException(nameof(audioDevices));
            Playlists = playlists ?? throw new ArgumentNullException(nameof(playlists));
            MediaPlayer = mediaPlayer ?? throw new ArgumentNullException(nameof(mediaPlayer));
        }
    }
}
