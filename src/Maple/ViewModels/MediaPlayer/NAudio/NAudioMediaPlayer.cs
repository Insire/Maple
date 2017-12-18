using System;
using Maple.Core;
using Maple.Domain;
using Maple.Localization.Properties;
using NAudio.Wave;

namespace Maple
{
    public sealed class NAudioMediaPlayer : BasePlayer
    {
        private readonly MediaFoundationReader.MediaFoundationReaderSettings _settings;

        private IWavePlayer _player;
        private WaveStream _reader;
        private VolumeWaveProvider16 _volumeProvider;

        public override int VolumeMax => 1;
        public override int VolumeMin => 0;

        private int _volume;
        public override int Volume
        {
            get { return _volume; }
            set { SetValue(ref _volume, value, OnChanged: () => SyncVolumeToVolumeProvider(value)); }
        }

        public NAudioMediaPlayer(ILoggingService log, IMessenger messenger, AudioDevices audioDevices, IWavePlayerFactory factory)
            : base(messenger, audioDevices)
        {
            _settings = new MediaFoundationReader.MediaFoundationReaderSettings
            {
                RepositionInRead = true,
                SingleReaderObject = false,
                RequestFloatOutput = false,
            };

            _player = factory.GetPlayer(log);
            _player.PlaybackStopped += PlaybackStopped;

            Messenger.Subscribe<PlayingMediaItemMessage>(OnPlaybackStarted);

            OnPropertyChanged(nameof(VolumeMin));
            OnPropertyChanged(nameof(VolumeMax));
        }

        private void OnPlaybackStarted(PlayingMediaItemMessage e)
        {
            Current = e.Content;
        }

        private void PlaybackStopped(object sender, StoppedEventArgs e)
        {
            Messenger.Publish(new CompletedMediaItemMessage(this, Current));
            Current = null;

            OnPropertyChanged(nameof(IsPlaying));
        }

        public override bool IsPlaying
        {
            get { return _player?.PlaybackState != null && _player.PlaybackState != NAudio.Wave.PlaybackState.Playing; }
        }

        public override bool CanPlay(IMediaItem item)
        {
            return item != null && _player?.PlaybackState != null && _player.PlaybackState != NAudio.Wave.PlaybackState.Playing && item.MediaItemType.HasFlag(MediaItemType.LocalFile);
        }

        public override bool CanPause()
        {
            return _player?.PlaybackState != null && _player.PlaybackState != NAudio.Wave.PlaybackState.Playing;
        }

        public override bool CanStop()
        {
            return _player?.PlaybackState != null && _player.PlaybackState != NAudio.Wave.PlaybackState.Playing;
        }

        public override void Pause()
        {
            if (_player == null)
                throw new ArgumentNullException(nameof(_player), $"{nameof(_player)} {Resources.IsRequired}");

            if (_player?.PlaybackState != NAudio.Wave.PlaybackState.Playing)
                throw new InvalidOperationException("Can't pause playback of a file, thats not being played back"); // TODO localize

            _player.Pause();
        }

        public override bool Play(IMediaItem mediaItem)
        {
            if (_player?.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                throw new InvalidOperationException("Can't play a file, when already playing"); // TODO localize

            _reader = new MediaFoundationReader(mediaItem.Location, _settings);

            _volumeProvider = new VolumeWaveProvider16(_reader)
            {
                Volume = 0.5f
            };
            _player.Init(_volumeProvider);
            _player.Play();

            return true;
        }

        public override void Stop()
        {
            if (_player == null)
                throw new ArgumentNullException(nameof(_player), $"{nameof(_player)} {Resources.IsRequired}");

            if (_player?.PlaybackState != NAudio.Wave.PlaybackState.Playing)
                return;

            _player.Stop();
        }

        private void SyncVolumeToVolumeProvider(int value)
        {
            if (_volumeProvider == null || value > 100 && value < 0)
                return;

            _volumeProvider.Volume = value / 100;
        }

        protected override void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (IsPlaying)
                Stop();

            if (disposing)
            {
                base.Dispose(disposing);

                if (_player != null)
                {
                    _player.PlaybackStopped -= PlaybackStopped;
                    _player?.Dispose();
                    _player = null;
                }

                if (_reader != null)
                {
                    _reader?.Close();
                    _reader?.Dispose();
                    _reader = null;
                }

                // Free any other managed objects here.
            }

            // Free any unmanaged objects here.
            Disposed = true;
        }
    }
}
