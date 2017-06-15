using Maple.Core;
using System;

namespace Maple
{
    public abstract class BasePlayer : ObservableObject, IMediaPlayer
    {
        protected readonly IMessenger _messenger;
        protected readonly ILoggingService _log;

        protected abstract void Dispose(bool disposing);

        public RangeObservableCollection<AudioDevice> Items { get; private set; }

        private AudioDevice _audioDevice;
        public AudioDevice AudioDevice
        {
            get { return _audioDevice; }
            set
            {
                SetValue(ref _audioDevice, value,
                                OnChanging: () => _messenger.Publish(new ViewModelSelectionChangingMessage<AudioDevice>(this, _audioDevice)),
                                OnChanged: () => _messenger.Publish(new ViewModelSelectionChangingMessage<AudioDevice>(this, value)));
            }
        }

        private IMediaItem _current;
        public IMediaItem Current
        {
            get { return _current; }
            set { SetValue(ref _current, value); }
        }

        private bool _disposed;
        public bool Disposed
        {
            get { return _disposed; }
            protected set { SetValue(ref _disposed, value); }
        }

        public BasePlayer(IMessenger messenger, AudioDevices audioDevices)
        {
            _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));

        }

        public abstract bool CanPlay(IMediaItem item);

        public abstract bool IsPlaying { get; }

        public abstract int Volume { get; set; }

        public abstract int VolumeMax { get; }

        public abstract int VolumeMin { get; }

        public abstract void Play(IMediaItem mediaItem);

        public abstract void Pause();

        public abstract void Stop();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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
