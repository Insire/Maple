using System;
using MvvmScarletToolkit;

namespace InsireBot
{
    public abstract class BasePlayer : BusinessViewModelBase<AudioDevice>, IMediaPlayer
    {
        public event CompletedMediaItemEventHandler CompletedMediaItem;

        public bool CanNext
        {
            get { return Playlist.CanNext(); }
        }

        public bool CanPrevious
        {
            get { return Playlist.CanPrevious(); }
        }

        public virtual bool CanPlay { get { return Playlist?.CurrentItem != null; } }
        public virtual bool CanPause { get { return Playlist?.CurrentItem != null; } }
        public virtual bool CanStop { get { return Playlist?.CurrentItem != null; } }

        private Playlist _playlist;
        public Playlist Playlist
        {
            get { return _playlist; }
            protected set { SetValue(ref _playlist, value); }
        }

        private RangeObservableCollection<AudioDevice> _audioDevices;
        public RangeObservableCollection<AudioDevice> AudioDevices
        {
            get { return _audioDevices; }
            set { SetValue(ref _audioDevices, value); }
        }

        private AudioDevice _audioDevice;
        public AudioDevice AudioDevice
        {
            get { return _audioDevice; }
            set { SetValue(ref _audioDevice, value); }
        }

        private bool _disposed;
        public bool Disposed
        {
            get { return _disposed; }
            protected set { SetValue(ref _disposed, value); }
        }

        public abstract bool IsPlaying { get; }

        public abstract int Volume { get; set; }

        public abstract int VolumeMax { get; }

        public abstract int VolumeMin { get; }

        public BasePlayer(IDataService dataService) : base(dataService)
        {
            AudioDevices = new RangeObservableCollection<AudioDevice>();
            CompletedMediaItem += Player_CompletedMediaItem;

            if (IsInDesignMode)
            {
                AudioDevices.AddRange(_dataService.GetPlaybackDevices());
            }
            else
            {
                AudioDevices.AddRange(_dataService.GetPlaybackDevices());
            }
        }

        protected virtual void Player_CompletedMediaItem(object sender, CompletedMediaItemEventEventArgs e)
        {
            CompletedMediaItem?.Invoke(this, e);
        }

        event CompletedMediaItemEventHandler IMediaPlayer.CompletedMediaItem
        {
            add { CompletedMediaItem += value; }
            remove { CompletedMediaItem -= value; }
        }

        public virtual void Play()
        {
            if (IsPlaying)
                Stop();

            if (Playlist?.CurrentItem != null)
                Play(Playlist.CurrentItem);

            Next();
        }

        public virtual void Next()
        {
            if (IsPlaying)
                Stop();

            var next = Playlist?.Next();
            if (!string.IsNullOrEmpty(next?.Location))
                Play(next);
        }

        public virtual void Previous()
        {
            if (IsPlaying)
                Stop();

            var previous = Playlist?.Previous();
            if (!string.IsNullOrEmpty(previous?.Location))
                Play(previous);
        }

        public abstract void Play(IMediaItem mediaItem);

        public abstract void Pause();

        public abstract void Stop();

        public abstract void Dispose();
    }
}
