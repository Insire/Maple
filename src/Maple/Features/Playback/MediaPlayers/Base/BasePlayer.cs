using Maple.Domain;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    public abstract class BasePlayer : ViewModelBase, IMediaPlayer
    {
        private IAudioDevice _audioDevice;
        public IAudioDevice AudioDevice
        {
            get { return _audioDevice; }
            set
            {
                SetValue(ref _audioDevice, value,
                                onChanging: () => Messenger.Publish(new ViewModelListBaseSelectionChanging<IAudioDevice>(this, _audioDevice)),
                                onChanged: () => Messenger.Publish(new ViewModelListBaseSelectionChanged<IAudioDevice>(this, value)));
            }
        }

        protected BasePlayer(IScarletCommandBuilder commandBuilder)
            : base(commandBuilder)
        {
        }

        private IMediaItem _current;
        public IMediaItem Current
        {
            get { return _current; }
            set { SetValue(ref _current, value); }
        }

        public abstract bool CanPlay(IMediaItem item);

        public abstract bool IsPlaying { get; }

        public abstract int Volume { get; set; }

        public abstract int VolumeMax { get; }

        public abstract int VolumeMin { get; }

        public abstract bool Play(IMediaItem item);

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
