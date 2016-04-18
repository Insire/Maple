using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

using InsireBotCore;

namespace InsireBot.ViewModel
{
    public class MediaPlayerViewModel : BotViewModelBase<IMediaItem>
    {
        public IMediaPlayer<IMediaItem> MediaPlayer { get; private set; }

        public bool IsPlaying { get { return MediaPlayer.IsPlaying; } }

        public ICommand PlayCommand { get; private set; }
        public ICommand NextCommand { get; private set; }
        public ICommand PreviousCommand { get; private set; }
        public ICommand AddCommand { get; private set; }

        public MediaPlayerViewModel(IDataService dataService) : base(dataService)
        {
            // receive MediaItems and add them to the playlist
            Messenger.Default.Register<MediaItem>(this, (mediaItem) =>
             {
                 Add(mediaItem);
             });

            Initialize(dataService);
        }

        private void MediaPlayer_CompletedMediaItem(object sender, CompletedMediaItemEventEventArgs e)
        {
            Next();
        }

        private void Initialize(IDataService dataService)
        {
            if (MediaPlayer != null)
            {
                MediaPlayer.CompletedMediaItem -= MediaPlayer_CompletedMediaItem;
                MediaPlayer.Dispose();
            }
            if (!Items.Any())
                Add(dataService.GetMediaItems()); // populate the playlist

            MediaPlayer = dataService.GetMediaPlayer();
            MediaPlayer.CompletedMediaItem += MediaPlayer_CompletedMediaItem;

            InitiliazeCommands();
        }

        private void InitiliazeCommands()
        {
            PlayCommand = new RelayCommand(Play);
            PreviousCommand = new RelayCommand(Previous, MediaPlayer.Playlist.CanPrevious);
            NextCommand = new RelayCommand(Next, MediaPlayer.Playlist.CanNext);
            AddCommand = new RelayCommand(OpenAddDialog);
        }

        private void Add(IEnumerable<IMediaItem> mediaItems)
        {
            foreach (var item in mediaItems)
                MediaPlayer.Playlist.Add(item);
        }

        private void Add(IMediaItem mediaItem)
        {
            if (Items.Any())
            {
                var maxIndex = Items.Max(p => p.Index) + 1;
                if (maxIndex < 0)
                    maxIndex = 0;

                mediaItem.Index = maxIndex;
            }
            else
            {
                mediaItem.Index = 0;
            }

            Items.Add(mediaItem);
        }

        private void OpenAddDialog()
        {
            var dialog = new NewMediaItemDialog();
            dialog.Owner = Application.Current.MainWindow;

            dialog.ShowDialog();
        }

        public void Previous()
        {
            Play();
        }

        public void Next()
        {
            MediaPlayer.Next();
        }

        public void Play()
        {
            MediaPlayer.Play();
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