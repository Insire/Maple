using GalaSoft.MvvmLight.Messaging;
using InsireBotCore;
using MvvmScarletToolkit;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace InsireBotWPF
{
    public class MediaPlayerViewModel : BusinessViewModelBase<IMediaItem>
    {
        public IMediaPlayer<IMediaItem> MediaPlayer { get; private set; }

        public bool IsPlaying { get { return MediaPlayer.IsPlaying; } }

        public ICommand PlayCommand { get; private set; }
        public ICommand NextCommand { get; private set; }
        public ICommand PreviousCommand { get; private set; }

        public Playlist<IMediaItem> Playlist
        {
            get { return MediaPlayer.Playlist; }
        }

        public MediaPlayerViewModel(IDataService dataService) : base(dataService)
        {
            // receive MediaItems and add them to the playlist
            Messenger.Default.Register<MediaItem>(this, (mediaItem) =>
             {
                 Items.Add(mediaItem);
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

            if (Items?.Any() != true)
            {
                Items.AddRange(dataService.GetMediaItems()); // populate the playlist
            }

            MediaPlayer = dataService.GetMediaPlayer();
            MediaPlayer.CompletedMediaItem += MediaPlayer_CompletedMediaItem;

            InitiliazeCommands();
        }

        private void InitiliazeCommands()
        {
            PlayCommand = new RelayCommand(Play, () => MediaPlayer.CanPlay);
            PreviousCommand = new RelayCommand(Previous, () => MediaPlayer.Playlist?.CanPrevious() == true);
            NextCommand = new RelayCommand(Next, () => MediaPlayer.Playlist?.CanNext() == true);
        }

        public override void AddRange(IEnumerable<IMediaItem> mediaItems)
        {
            foreach (var item in mediaItems)
                MediaPlayer.Playlist.Add(item);
        }

        public override void Add(IMediaItem mediaItem)
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

        public void Previous()
        {
            MediaPlayer.Previous();
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

        public void SetPlaylist(Playlist<IMediaItem> playlist)
        {
            MediaPlayer.Playlist.Clear();
            var items = MediaPlayer.Playlist.CanAddRange(playlist);
            MediaPlayer.Playlist.AddRange(items);
        }
    }
}