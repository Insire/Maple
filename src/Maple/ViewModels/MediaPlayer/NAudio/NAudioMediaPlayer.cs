using System;
using Maple.Core;
using Maple.Interfaces;
using Maple.Localization.Properties;
using NAudio.Wave;

namespace Maple
{
    public class NAudioMediaPlayer : BasePlayer
    {
        private readonly MediaFoundationReader.MediaFoundationReaderSettings _settings;

        private int _volume;
        private IMediaItem _current;
        private IWavePlayer _player;
        private WaveStream _reader;
        private VolumeWaveProvider16 _volumeProvider;

        public override int VolumeMax => 1;
        public override int VolumeMin => 0;

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

            _messenger.Subscribe<PlayingMediaItemMessage>(OnPlaybackStarted);

            OnPropertyChanged(nameof(VolumeMin));
            OnPropertyChanged(nameof(VolumeMax));
        }

        private void OnPlaybackStarted(PlayingMediaItemMessage e)
        {
            _current = e.Content;
        }

        private void PlaybackStopped(object sender, StoppedEventArgs e)
        {
            _messenger.Publish(new CompletedMediaItemMessage(this, _current));
            _current = null;

            OnPropertyChanged(nameof(IsPlaying));
            OnPropertyChanged(nameof(CanPlay));
            OnPropertyChanged(nameof(CanStop));
            OnPropertyChanged(nameof(CanPause));
        }

        public override bool IsPlaying
        {
            get { return _player?.PlaybackState != null && _player.PlaybackState != NAudio.Wave.PlaybackState.Playing; }
        }

        public override bool CanPlay(IMediaItem item)
        {
            return item != null && _player?.PlaybackState != null && _player.PlaybackState != NAudio.Wave.PlaybackState.Playing;
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
                throw new InvalidOperationException("Can't pause playback of a file, thats not being played back");

            _player.Pause();
        }

        public override void Play(IMediaItem mediaItem)
        {
            if (_player?.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                throw new InvalidOperationException("Can't play a file, when already playing");

            _reader = new MediaFoundationReader(mediaItem.Location, _settings);

            _volumeProvider = new VolumeWaveProvider16(_reader)
            {
                Volume = 0.5f
            };
            _player.Init(_volumeProvider);
            _player.Play();

            _messenger.Publish(new PlayingMediaItemMessage(this, mediaItem));
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
