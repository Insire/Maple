namespace InsireBotCore
{
    public abstract class BasePlayer : BusinessViewModelBase<AudioDevice>, IMediaPlayer<IMediaItem>
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

        private Playlist<IMediaItem> _playlist;
        public Playlist<IMediaItem> Playlist
        {
            get { return _playlist; }
            protected set
            {
                if (_playlist != value && value != null)
                {
                    _playlist = value;
                    RaisePropertyChanged(nameof(Playlist));
                }
            }
        }

        private RangeObservableCollection<AudioDevice> _audioDevices;
        public RangeObservableCollection<AudioDevice> AudioDevices
        {
            get { return _audioDevices; }
            set
            {
                if (_audioDevices != value && value != null)
                {
                    _audioDevices = value;
                    RaisePropertyChanged(nameof(AudioDevices));
                }
            }
        }

        private AudioDevice _audioDevice;
        public AudioDevice AudioDevice
        {
            get { return _audioDevice; }
            set
            {
                if (_audioDevice != value && value != null)
                {
                    _audioDevice = value;
                    RaisePropertyChanged(nameof(AudioDevice));
                }
            }
        }

        private bool _disposed;
        public bool Disposed
        {
            get { return _disposed; }
            protected set
            {
                _disposed = value;
                RaisePropertyChanged(nameof(Disposed));
            }
        }

        public abstract bool IsPlaying { get; }

        public abstract int Volume { get; set; }

        public abstract bool Silent { get; set; }

        public int VolumeMax { get; protected set; }

        public int VolumeMin { get; protected set; }

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

        event CompletedMediaItemEventHandler IMediaPlayer<IMediaItem>.CompletedMediaItem
        {
            add { CompletedMediaItem += value; }
            remove { CompletedMediaItem -= value; }
        }

        public void Play()
        {
            if (IsPlaying)
                Stop();

            if (Playlist.CurrentItem != null)
                Play(Playlist.CurrentItem);

            Next();
        }

        public void Next()
        {
            if (IsPlaying)
                Stop();

            var next = Playlist.Next();
            if (next != null)
                Play(next);
        }

        public void Previous()
        {
            if (IsPlaying)
                Stop();

            var previous = Playlist.Previous();
            if (previous != null)
                Play(previous);
        }

        public abstract void Play(IMediaItem mediaItem);

        public abstract void Pause();

        public abstract void Stop();

        public abstract void Dispose();
    }
}
