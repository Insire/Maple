using System;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using InsireBot.MediaPlayer;

namespace InsireBot.ViewModel
{
    public class MediaPlayerViewModel : BotViewModelBase<MediaItem>, IPlaying
    {
        public IMediaPlayer<IMediaItem> MediaPlayer { get; }

        public ICommand PlayCommand { get; private set; }
        public ICommand NextCommand { get; private set; }
        public ICommand PreviousCommand { get; private set; }
        public ICommand AddCommand { get; private set; }

        public MediaPlayerViewModel(IDataService dataService) : base(dataService)
        {
            if (IsInDesignMode)
            {
                var item = new MediaItem("Rusko - Somebody To Love (Sigma Remix)", @"https://www.youtube.com/watch?v=nF7wa3j57j0", new TimeSpan(0, 5, 47));
                Items.Add(item);

                item = new MediaItem("Armin van Buuren feat. Sophie - Virtual Friend", @"https://www.youtube.com/watch?v=0ypeOKp0x3k", new TimeSpan(0, 7, 12));
            }
            else
            {
                // Code runs "for real"
                MediaPlayer = MediaPlayerFactory.Create(dataService, MediaPlayerType.VLCDOTNET);
                var item = new MediaItem("Rusko - Somebody To Love (Sigma Remix)", @"https://www.youtube.com/watch?v=nF7wa3j57j0", new TimeSpan(0, 5, 47));
                Items.Add(item);

                item = new MediaItem("Armin van Buuren feat. Sophie - Virtual Friend", @"https://www.youtube.com/watch?v=0ypeOKp0x3k", new TimeSpan(0, 7, 12));
                Items.Add(item);

                item = new MediaItem("Will & Tim ft. Ephixa - Stone Tower Temple", "C:\\Users\\Insire\\Downloads\\Will & Tim ft. Ephixa - Stone Tower Temple.mp3");
                Items.Add(item);
                // receive MediaItems and add them to the playlist
                Messenger.Default.Register<MediaItem>(this, (mediaItem) =>
                 {
                     Items.Add(mediaItem);
                 });

                InitiliazeCommands();
            }
        }

        private void InitiliazeCommands()
        {
            PlayCommand = new RelayCommand(Play);
            PreviousCommand = new RelayCommand(Previous, MediaPlayer.CanPrevious);
            NextCommand = new RelayCommand(Next, MediaPlayer.CanNext);
            AddCommand = new RelayCommand(Add);
        }

        private void Add()
        {
            var dialog = new NewMediaItemDialog();
            dialog.Owner = Application.Current.MainWindow;

            dialog.ShowDialog();
        }

        public void Next()
        {
            SelectedIndex++;
            var contract = new MediaItemContract
            {
                MediaItem = SelectedItem,
                Action = MediaItemAction.Play,
            };

            Messenger.Default.Send(contract);
        }

        public void Previous()
        {
            SelectedIndex--;
            var contract = new MediaItemContract
            {
                MediaItem = SelectedItem,
                Action = MediaItemAction.Play,
            };

            Messenger.Default.Send(contract);
        }

        public void Play(IMediaItem item)
        {
            MediaPlayer.Play(item);
        }

        public void Play()
        {
            //var contract = new MediaItemContract
            //{
            //    MediaItem = SelectedItem,
            //    Action = MediaItemAction.Play,
            //};

            //Messenger.Default.Send(contract);
            MediaPlayer.Play(SelectedItem);
        }

        public void Pause()
        {
            var contract = new MediaItemContract
            {
                MediaItem = SelectedItem,
                Action = MediaItemAction.Pause,
            };

            Messenger.Default.Send(contract);
        }

        public void Stop()
        {
            var contract = new MediaItemContract
            {
                MediaItem = SelectedItem,
                Action = MediaItemAction.Stop,
            };

            Messenger.Default.Send(contract);
        }
    }
}