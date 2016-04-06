using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using InsireBot.MediaPlayer;

namespace InsireBot.ViewModel
{
    public class MediaPlayerViewModel : BotViewModelBase<MediaItem>
    {
        public Stack<int> PlayedList { get; private set; } // contains indices of played mediaItems
        public IMediaPlayer<IMediaItem> MediaPlayer { get; }

        public bool IsPlaying { get { return MediaPlayer.IsPlaying; } }

        public ICommand PlayCommand { get; private set; }
        public ICommand NextCommand { get; private set; }
        public ICommand PreviousCommand { get; private set; }
        public ICommand AddCommand { get; private set; }

        public IMediaItem NextMediaItem { get; private set; }
        public IMediaItem PreviousMediaItem { get; private set; }

        public MediaPlayerViewModel(IDataService dataService) : base(dataService)
        {
            if (IsInDesignMode)
            {
                var item = new MediaItem("Rusko - Somebody To Love (Sigma Remix)", new Uri(@"https://www.youtube.com/watch?v=nF7wa3j57j0"), new TimeSpan(0, 5, 47));
                Items.Add(item);

                item = new MediaItem("Armin van Buuren feat. Sophie - Virtual Friend", new Uri(@"https://www.youtube.com/watch?v=0ypeOKp0x3k"), new TimeSpan(0, 7, 12));
            }
            else                 // Code runs "for real"
            {
                PlayedList = new Stack<int>();

                MediaPlayer = MediaPlayerFactory.Create(dataService, MediaPlayerType.VLCDOTNET);
                MediaPlayer.CompletedMediaItem += MediaPlayer_CompletedMediaItem;

                var item = new MediaItem("Rusko - Somebody To Love (Sigma Remix)", new Uri(@"https://www.youtube.com/watch?v=nF7wa3j57j0"), new TimeSpan(0, 5, 47));
                Items.Add(item);

                item = new MediaItem("Armin van Buuren feat. Sophie - Virtual Friend", new Uri(@"https://www.youtube.com/watch?v=0ypeOKp0x3k"), new TimeSpan(0, 7, 12));
                Items.Add(item);

                item = new MediaItem("Will & Tim ft. Ephixa - Stone Tower Temple", new Uri("C:\\Users\\Insire\\Downloads\\Will & Tim ft. Ephixa - Stone Tower Temple.mp3"));
                Items.Add(item);

                // receive MediaItems and add them to the playlist
                Messenger.Default.Register<MediaItem>(this, (mediaItem) =>
                 {
                     Add(mediaItem);
                 });

                InitiliazeCommands();
            }
        }

        private void MediaPlayer_CompletedMediaItem(object sender, CompletedMediaItemEventEventArgs e)
        {
            // TODO continous playback
            throw new NotImplementedException();
        }

        private void InitiliazeCommands()
        {
            PlayCommand = new RelayCommand(Play);
            PreviousCommand = new RelayCommand(Previous, CanPrevious);
            NextCommand = new RelayCommand(Next, CanNext);
            AddCommand = new RelayCommand(AddWithDialog);
        }

        private void Add(MediaItem mediaItem)
        {
            var maxIndex = Items.Max(p => p.Index) + 1;
            if (maxIndex < 0)
                maxIndex = 0;

            mediaItem.Index = maxIndex;
            Items.Add(mediaItem);
        }

        private void AddWithDialog()
        {
            var dialog = new NewMediaItemDialog();
            dialog.Owner = Application.Current.MainWindow;

            dialog.ShowDialog();
        }

        private bool CanPrevious()
        {
            return PreviousMediaItem != null;
        }

        private void SelectPrevious()
        {
            if (PlayedList != null && PlayedList.Any())
            {
                var foundItemInPlaylist = false;
                var previous = PlayedList.Pop();
                if (previous > -1)
                {
                    while (!foundItemInPlaylist)
                    {
                        var previousItems = Items.Where(p => p.Index == previous); // try to get the last played item
                        if (previousItems.Any()) // success
                        {

                            Items.ToList().ForEach(p => p.IsSelected = false);      // deselect all items in the list
                            previousItems.First().IsSelected = true;                // select the one we just found
                            foundItemInPlaylist = true;                             // set flag to leave this

                            if (previousItems.Count() > 1)
                                Debug.WriteLine("Warning SelectPrevious returned more than one value, when it should only return one");
                        }
                    }
                }
            }
        }

        public void Previous()
        {
            SelectPrevious();
            MediaPlayer.Play(SelectedItem);
        }

        private bool CanNext()
        {
            return NextMediaItem != null;
        }

        private void SelectNext()
        {
            // TODO

        }

        public void Next()
        {
            SelectNext();
            MediaPlayer.Play(SelectedItem);
        }

        public void Play(IMediaItem item)
        {
            MediaPlayer.Play(item);
        }

        public void Play()
        {
            if (!MediaPlayer.IsPlaying)
            {
                MediaPlayer.Play(SelectedItem);

            }
            else
                Stop();
        }

        public void Pause()
        {
            MediaPlayer.Pause();
        }

        public void Stop()
        {
            MediaPlayer.Stop();
        }
    }
}