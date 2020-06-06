using System.Linq;
using Maple.Domain;
using MvvmScarletToolkit;

namespace Maple
{
    public sealed class MediaPlayerFactory
    {
        private readonly IScarletCommandBuilder _commandBuilder;
        private readonly AudioDevices _audioDevices;
        private readonly Playlists _playlists;

        public MediaPlayerFactory(IScarletCommandBuilder commandBuilder, AudioDevices audioDevices, Playlists playlists)
        {
            _commandBuilder = commandBuilder ?? throw new System.ArgumentNullException(nameof(commandBuilder));
            _audioDevices = audioDevices ?? throw new System.ArgumentNullException(nameof(audioDevices));
            _playlists = playlists ?? throw new System.ArgumentNullException(nameof(playlists));
        }

        public MediaPlayer Create(MediaPlayerModel model)
        {
            var playlist = _playlists.Items.FirstOrDefault(p => p.Id == model.PlaylistId);

            var result = new MediaPlayer(_commandBuilder, model, playlist, null);

            return result;
        }

        public CreateMediaPlayerViewModel Create()
        {
            var playerViewModel = new MediaPlayer(_commandBuilder, new MaplePlaybackService(_commandBuilder));

            return Create(playerViewModel);
        }

        public CreateMediaPlayerViewModel Create(MediaPlayer mediaPlayer)
        {
            return new CreateMediaPlayerViewModel(_audioDevices, _playlists, mediaPlayer);
        }
    }
}
