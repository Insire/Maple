using System;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using InsireBot.MediaPlayer;

namespace InsireBot.ViewModel
{
    public class MediaPlayerViewModel : BotViewModelBase<MediaItem>, IPlaying
    {
        public IMediaPlayer<IMediaItem> MediaPlayer { get; }

        public MediaPlayerViewModel(IDataService dataService) : base(dataService)
        {
            if (IsInDesignMode)
            {
                var item = new MediaItem("Rusko - Somebody To Love (Sigma Remix)", @"https://www.youtube.com/watch?v=nF7wa3j57j0", new TimeSpan(0, 5, 47));
                Items.Add(item);

                item = new MediaItem("Armin van Buuren feat. Sophie - Virtual Friend", @"https://www.youtube.com/watch?v=0ypeOKp0x3k", new TimeSpan(0, 7, 12));
                Items.Add(item);

                Items.Add(item);
            }
            else
            {
                // Code runs "for real"
                MediaPlayer = MediaPlayerFactory.Create(dataService, MediaPlayerType.VLCDOTNET);
                var item = new MediaItem("Rusko - Somebody To Love (Sigma Remix)", @"https://www.youtube.com/watch?v=nF7wa3j57j0", new TimeSpan(0, 5, 47));
                Items.Add(item);

                item = new MediaItem("Armin van Buuren feat. Sophie - Virtual Friend", @"https://www.youtube.com/watch?v=0ypeOKp0x3k", new TimeSpan(0, 7, 12));
                Items.Add(new MediaItem("1","11"));
                Items.Add(new MediaItem("2", "22"));
                Items.Add(new MediaItem("3", "33"));
                Items.Add(new MediaItem("4", "44"));
                Items.Add(new MediaItem("5", "55"));
                Items.Add(new MediaItem("6", "66"));

                // receive MediaItems send to the Messenger
                Messenger.Default.Register<MediaItem>(this, (mediaItem) =>
                 {
                     Items.Add(mediaItem);
                 });

                NewCommand = new RelayCommand(New);
            }
        }

        private void New()
        {
            var dialog = new NewMediaItemDialog();
            dialog.Owner = Application.Current.MainWindow;

            var result = dialog.ShowDialog();
        }

        public void Next()
        {
            SelectedIndex++;
            var contract = new MediaItemContract
            {
                MediaItem = SelectedItem,
                Action = MediaItemAction.Next,
            };

            Messenger.Default.Send(contract);
        }

        public void Previous()
        {
            SelectedIndex--;
            var contract = new MediaItemContract
            {
                MediaItem = SelectedItem,
                Action = MediaItemAction.Previous,
            };

            Messenger.Default.Send(contract);
        }

        public void Play()
        {
            var contract = new MediaItemContract
            {
                MediaItem = SelectedItem,
                Action = MediaItemAction.Play,
            };

            Messenger.Default.Send(contract);
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