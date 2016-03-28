using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
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

        private MediaPlayPlaybackType _mediaPlayPlaybackType;
        public MediaPlayPlaybackType MediaPlayPlaybackType
        {
            get { return _mediaPlayPlaybackType; }
            set
            {
                if (_mediaPlayPlaybackType != value)
                {
                    _mediaPlayPlaybackType = value;
                    RaisePropertyChanged(nameof(MediaPlayPlaybackType));
                }
            }
        }
        private void Test()
        {
            MediaItemFactory.Create(@"i2n0e6vyziY");
        }
        public ICommand TestCommand { get; }
        public BasePlayer(IDataService dataService) : base(dataService)
        {
            TestCommand = new RelayCommand(Test);
            AudioDevices = new RangeObservableCollection<AudioDevice>();
            MediaPlayPlaybackType = MediaPlayPlaybackType.Play;

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
