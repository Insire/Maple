using NAudio.Wave;
using System;

namespace InsireBot
{
    public class NAudioMediaPlayer : BasePlayer
    {
        private IWavePlayer _player;
        private WaveStream _reader;
        private VolumeWaveProvider16 _volumeProvider;
        private MediaFoundationReader.MediaFoundationReaderSettings _settings;

        public override int VolumeMax => 1;
        public override int VolumeMin => 0;

        private int _volume;
        public override int Volume
        {
            get { return _volume; }
            set { SetValue(ref _volume, value, () => SyncVolumeToVolumeProvider(value)); }
        }

        public NAudioMediaPlayer(IDataService dataService) : base(dataService)
        {
            _settings = new MediaFoundationReader.MediaFoundationReaderSettings
            {
                RepositionInRead = true,
                SingleReaderObject = false,
                RequestFloatOutput = false,
            };

            _player = WavePlayerFactory.GetPlayer();
            _player.PlaybackStopped += PlaybackStopped;

            OnPropertyChanged(nameof(VolumeMin));
            OnPropertyChanged(nameof(VolumeMax));
        }

        private void PlaybackStopped(object sender, StoppedEventArgs e)
        {
            OnPropertyChanged(nameof(IsPlaying));
            OnPropertyChanged(nameof(CanPlay));
            OnPropertyChanged(nameof(CanStop));
            OnPropertyChanged(nameof(CanPause));
        }

        public override bool CanPlay
        {
            get { return base.CanPlay && _player?.PlaybackState != null && _player?.PlaybackState != PlaybackState.Playing; }
        }

        public override bool CanPause
        {
            get { return base.CanPlay && _player?.PlaybackState == PlaybackState.Playing; }
        }

        public override bool IsPlaying
        {
            get { return base.CanPlay && _player?.PlaybackState == PlaybackState.Playing; }
        }

        public override bool CanStop
        {
            get { return base.CanPlay && _player?.PlaybackState == PlaybackState.Playing; }
        }

        public override void Pause()
        {
            if (_player == null)
                throw new ArgumentNullException($"{nameof(_player)} is null");

            if (_player?.PlaybackState != PlaybackState.Playing)
                throw new InvalidOperationException("Can't pause playback of a file, thats not being played back");

            _player.Pause();
        }

        public override void Play(IMediaItem mediaItem)
        {
            if (_player?.PlaybackState == PlaybackState.Playing)
                throw new InvalidOperationException("Can't play a file, when already playing");

            var t = new WaveFileReader("");
            _reader = new MediaFoundationReader(mediaItem.Location, _settings);

            _volumeProvider = new VolumeWaveProvider16(_reader);
            _volumeProvider.Volume = 0.5f;

            _player.Init(_volumeProvider);
            _player.Play();
        }

        public override void Stop()
        {
            if (_player == null)
                throw new ArgumentNullException($"{nameof(_player)} is null");

            if (_player?.PlaybackState != PlaybackState.Playing)
                throw new InvalidOperationException("Can't stop playback for a file, thats not being played back");

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

        public void Dispose(bool disposing)
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

                _reader?.Dispose();
                _reader = null;

                // Free any other managed objects here.
            }

            // Free any unmanaged objects here.
            Disposed = true;
        }
    }
}
