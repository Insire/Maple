using Maple.Domain;

namespace Maple.Core
{
    public abstract class BasePlayer : ViewModel, IMediaPlayer
    {
        protected readonly ILoggingService _log;

        private IRangeObservableCollection<AudioDevice> _items;
        public IRangeObservableCollection<AudioDevice> Items
        {
            get { return _items; }
            private set { SetValue(ref _items, value); }
        }

        private IAudioDevice _audioDevice;
        public IAudioDevice AudioDevice
        {
            get { return _audioDevice; }
            set
            {
                SetValue(ref _audioDevice, value,
                                OnChanging: () => Messenger.Publish(new ViewModelSelectionChangingMessage<IAudioDevice>(this, _audioDevice)),
                                OnChanged: () => Messenger.Publish(new ViewModelSelectionChangingMessage<IAudioDevice>(this, value)));
            }
        }

        private IMediaItem _current;
        public IMediaItem Current
        {
            get { return _current; }
            set { SetValue(ref _current, value); }
        }

        protected BasePlayer(IMessenger messenger, AudioDevices audioDevices)
            : base(messenger)
        {
        }

        public abstract bool CanPlay(IMediaItem item);

        public abstract bool IsPlaying { get; }

        public abstract int Volume { get; set; }

        public abstract int VolumeMax { get; }

        public abstract int VolumeMin { get; }

        public abstract bool Play(IMediaItem mediaItem);

        public abstract void Pause();

        public abstract void Stop();

        public virtual bool CanStop()
        {
            return false;
        }

        public virtual bool CanPause()
        {
            return false;
        }
    }
}
