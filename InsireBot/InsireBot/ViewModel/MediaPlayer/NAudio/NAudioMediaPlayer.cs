using Maple.Core;
using NAudio.Wave;
using System;

namespace Maple
{
    public class NAudioMediaPlayer : BasePlayer
    {
        private readonly IMapleLog _log;
        private readonly MediaFoundationReader.MediaFoundationReaderSettings _settings;

        private int _volume;
        private IMediaItem _current;
        private IWavePlayer _player;
        private WaveStream _reader;
        private VolumeWaveProvider16 _volumeProvider;

        public override int VolumeMax => 1;
        public override int VolumeMin => 0;

        public override event PlayingMediaItemEventHandler PlayingMediaItem;
        public override event CompletedMediaItemEventHandler CompletedMediaItem;

        public override int Volume
        {
            get { return _volume; }
            set { SetValue(ref _volume, value, OnChanged: () => SyncVolumeToVolumeProvider(value)); }
        }

        public NAudioMediaPlayer(IMapleLog log) : base()
        {
            _log = log;
            _settings = new MediaFoundationReader.MediaFoundationReaderSettings
            {
                RepositionInRead = true,
                SingleReaderObject = false,
                RequestFloatOutput = false,
            };

            AudioDeviceChanging += OnAudioDeviceChanging;
            AudioDeviceChanged += OnAudioDeviceChanged;

            _player = WavePlayerFactory.GetPlayer();
            _player.PlaybackStopped += PlaybackStopped;

            PlayingMediaItem += OnPlaybackStarted;

            OnPropertyChanged(nameof(VolumeMin));
            OnPropertyChanged(nameof(VolumeMax));
        }

        private void OnPlaybackStarted(object sender, PlayingMediaItemEventArgs e)
        {
            _current = e.MediaItem;
        }

        private void OnAudioDeviceChanging(object sender, EventArgs e)
        {

        }

        private void OnAudioDeviceChanged(object sender, AudioDeviceChangedEventArgs e)
        {

        }

        private void PlaybackStopped(object sender, StoppedEventArgs e)
        {
            CompletedMediaItem?.Invoke(this, new CompletedMediaItemEventEventArgs(_current));

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
                throw new ArgumentNullException($"{nameof(_player)} is null");

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

            PlayingMediaItem?.Invoke(this, new PlayingMediaItemEventArgs(mediaItem));
        }

        public override void Stop()
        {
            if (_player == null)
                throw new ArgumentNullException($"{nameof(_player)} is null");

            if (_player?.PlaybackState != NAudio.Wave.PlaybackState.Playing)
                return;

            _player.Stop();
        }

        private void SyncVolumeToVolumeProvider(int value)
        {
            if (_volumeProvider == null && value <= 100 && value > 0)
                return;

            _volumeProvider.Volume = value / 100;
        }

        public sealed override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (IsPlaying)
                Stop();

            if (disposing)
            {
                _player.PlaybackStopped -= PlaybackStopped;
                _player?.Dispose();
                _player = null;

                _reader?.Close();
                _reader?.Dispose();
                _reader = null;

                // Free any other managed objects here.
            }

            // Free any unmanaged objects here.
            Disposed = true;
        }
    }
}
