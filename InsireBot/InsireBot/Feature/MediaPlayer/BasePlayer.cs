using System;
using GalaSoft.MvvmLight.Messaging;

namespace InsireBot.MediaPlayer
{
    public abstract class BasePlayer : BotViewModelBase<IMediaItem>, IPlaying
    {
        public abstract AudioDevice AudioDevice { get; set; }

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

            Messenger.Default.Register<MediaItemContract>(this, (mediaItemContract) =>
            {
                switch (mediaItemContract.Action)
                {
                    case MediaItemAction.Next:
                        if (mediaItemContract.MediaItem != null)
                            Next(mediaItemContract.MediaItem);

                        Next();
                        break;

                    case MediaItemAction.Previous:
                        if (mediaItemContract.MediaItem != null)
                            Previous(mediaItemContract.MediaItem);

                        Previous();
                        break;

                    case MediaItemAction.Play:
                        if (mediaItemContract.MediaItem != null)
                            Play(mediaItemContract.MediaItem);

                        Play();
                        break;

                    case MediaItemAction.Pause:
                        Pause();
                        break;

                    case MediaItemAction.Stop:
                        Stop();
                        break;

                    default:
                        throw new NotImplementedException();
                }
            });
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

        public void Add(IMediaItem item)
        {
            Items.Add(item);
        }
        public void Remove(IMediaItem item)
        {
            Items.Remove(item);
        }

        public abstract void Next(IMediaItem item);

        public abstract void Previous(IMediaItem item);

        public abstract void Play(IMediaItem item);

        public abstract void Next();

        public abstract void Previous();

        public abstract void Play();

        public abstract void Pause();

        public abstract void Stop();
    }
}
