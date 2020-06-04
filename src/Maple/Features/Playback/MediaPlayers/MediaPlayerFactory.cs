using MvvmScarletToolkit;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    public sealed class MediaPlayerFactory : ViewModelBase
    {
        private readonly AudioDevices _audioDevices;
        private readonly Playlists _playlists;

        public MediaPlayerFactory(IScarletCommandBuilder commandBuilder, AudioDevices audioDevices, Playlists playlists)
            : base(commandBuilder)
        {
            _audioDevices = audioDevices ?? throw new System.ArgumentNullException(nameof(audioDevices));
            _playlists = playlists ?? throw new System.ArgumentNullException(nameof(playlists));
        }

        public CreateMediaPlayerViewModel Create()
        {
            var playerViewModel = new MediaPlayer(CommandBuilder, new MaplePlayer(CommandBuilder));

            return new CreateMediaPlayerViewModel(_audioDevices, _playlists, playerViewModel);
        }
    }
}
