using System;
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
        private readonly Func<IPlaybackService> _playbackServiceFactory;

        public MediaPlayerFactory(IScarletCommandBuilder commandBuilder, AudioDevices audioDevices, Playlists playlists, Func<IPlaybackService> playbackServiceFactory)
        {
            _commandBuilder = commandBuilder ?? throw new ArgumentNullException(nameof(commandBuilder));
            _audioDevices = audioDevices ?? throw new ArgumentNullException(nameof(audioDevices));
            _playlists = playlists ?? throw new ArgumentNullException(nameof(playlists));
            _playbackServiceFactory = playbackServiceFactory ?? throw new ArgumentNullException(nameof(playbackServiceFactory));
        }

        public MediaPlayer Create(MediaPlayerModel model)
        {
            var playlist = _playlists.Items.FirstOrDefault(p => p.Id == model.PlaylistId) ?? _playlists.Items.FirstOrDefault();
            var audioDevice = _audioDevices.Items.FirstOrDefault(p => p.Id == model.AudioDeviceId) ?? _audioDevices.Items.FirstOrDefault();

            var playbackServiceFactory = _playbackServiceFactory();

            var result = new MediaPlayer(_commandBuilder, model, playlist, audioDevice, playbackServiceFactory);

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
