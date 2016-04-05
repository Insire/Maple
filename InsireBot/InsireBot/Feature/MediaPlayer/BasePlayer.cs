namespace InsireBot.MediaPlayer
{
    public abstract class BasePlayer : BotViewModelBase<AudioDevice>
    {
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

        private IMediaItem _current;
        public IMediaItem Current
        {
            get { return _current; }
            set
            {
                if (_current != value)
                {
                    _current = value;
                    RaisePropertyChanged(nameof(Current));
                }
            }
        }

        private RepeatMode _repeatMode;
        public RepeatMode RepeatMode
        {
            get { return _repeatMode; }
            set
            {
                if (_repeatMode != value)
                {
                    _repeatMode = value;
                    RaisePropertyChanged(nameof(RepeatMode));
                }
            }
        }

        private bool _shuffle;
        public bool Shuffle
        {
            get { return _shuffle; }
            set
            {
                if (_shuffle != value)
                {
                    _shuffle = value;
                    RaisePropertyChanged(nameof(Shuffle));
                }
            }
        }

        public BasePlayer(IDataService dataService) : base(dataService)
        {
            AudioDevices = new RangeObservableCollection<AudioDevice>();
            RepeatMode = RepeatMode.None;

            if (IsInDesignMode)
            {
                AudioDevices.AddRange(_dataService.GetPlaybackDevices());
            }
            else
            {
                AudioDevices.AddRange(_dataService.GetPlaybackDevices());
            }
        }

        public bool CanNext()
        {
            var next = SelectedIndex++;
            return next > -1 && next < Count();
        }

        public bool CanPlay()
        {
            return CanRemove();
        }

        public bool CanPrevious()
        {
            var next = SelectedIndex--;
            return next > -1 && next < Count();
        }
    }
}
