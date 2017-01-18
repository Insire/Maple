using GalaSoft.MvvmLight.Messaging;
using MvvmScarletToolkit;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace InsireBot
{
    public class MediaPlayerViewModel : BusinessViewModelBase<IMediaItem>
    {
        public IMediaPlayer MediaPlayer { get; private set; }

        public bool IsPlaying { get { return MediaPlayer.IsPlaying; } }

        public ICommand PlayCommand { get; private set; }
        public ICommand PauseCommand { get; private set; }
        public ICommand NextCommand { get; private set; }
        public ICommand PreviousCommand { get; private set; }

        public Playlist Playlist
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
            PauseCommand = new RelayCommand(Pause,()=> MediaPlayer.CanPause);
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
                var maxIndex = Items.Max(p => p.Sequence) + 1;
                if (maxIndex < 0)
                    maxIndex = 0;

                mediaItem.Sequence = maxIndex;
            }
            else
            {
                mediaItem.Sequence = 0;
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

        public void SetPlaylist(Playlist playlist)
        {
            MediaPlayer.Playlist.Clear();
            var items = MediaPlayer.Playlist.CanAddRange(playlist);
            MediaPlayer.Playlist.AddRange(items);
        }
    }
}