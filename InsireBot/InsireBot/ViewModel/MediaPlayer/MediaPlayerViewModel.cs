using InsireBot.Core;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System;

namespace InsireBot
{
    public class MediaPlayerViewModel : ViewModelListBase<MediaItemViewModel>, IValidatableTrackingObject
    {
        public IMediaPlayer Player { get; private set; }

        public bool IsPlaying { get { return Player.IsPlaying; } }

        public ICommand PlayCommand { get; private set; }
        public ICommand PauseCommand { get; private set; }
        public ICommand NextCommand { get; private set; }
        public ICommand PreviousCommand { get; private set; }

        private PlaylistViewModel _playlist;
        public PlaylistViewModel Playlist
        {
            get { return _playlist; }
            private set { SetValue(ref _playlist, value, Changing: OnPlaylistChanging, Changed: OnPlaylistChanged); }
        }

        public bool IsValid { get; }

        public bool IsChanged { get; }

        public MediaPlayerViewModel(IMediaPlayer player) : base()
        {
            Player = player;
            Player.PlayingMediaItem += Player_PlayingMediaItem;
            Player.CompletedMediaItem += MediaPlayer_CompletedMediaItem;

            InitiliazeCommands();
        }

        public void SetPlaylist(PlaylistViewModel playlist)
        {
            // TODO
        }

        private void OnPlaylistChanging()
        {
            Stop();
        }

        private void OnPlaylistChanged()
        {
            // TODO: maybe add optional endless playback
        }

        private void Player_PlayingMediaItem(object sender, PlayingMediaItemEventArgs e)
        {
            // TODO: sync state to other viewmodels
        }

        private void MediaPlayer_CompletedMediaItem(object sender, CompletedMediaItemEventEventArgs e)
        {
            Next();
        }

        private void InitiliazeCommands()
        {
            PlayCommand = new RelayCommand<MediaItemViewModel>(Player.Play, CanPlay);
            PreviousCommand = new RelayCommand(Previous, () => Playlist?.CanPrevious() == true);
            NextCommand = new RelayCommand(Next, () => Playlist?.CanNext() == true);
            PauseCommand = new RelayCommand(Pause, () => Player.CanPause());
        }

        public override void AddRange(IEnumerable<MediaItemViewModel> mediaItems)
        {
            foreach (var item in mediaItems)
                Playlist.Add(item);
        }

        public override void Add(MediaItemViewModel mediaItem)
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

        public void Pause()
        {
            Player.Pause();
        }

        public void Stop()
        {
            Player.Stop();
        }

        public void Previous()
        {
            var item = Playlist.Previous();
            Player.Play(item);
        }

        public void Next()
        {
            var item = Playlist.Next();
            Player.Play(item);
        }

        public void CanNext()
        {
            var item = Playlist.Next();
            Player.CanPlay(item);
        }

        private bool CanPlay(MediaItemViewModel item)
        {
            return false;
        }

        public void RejectChanges()
        {
            throw new NotImplementedException();
        }

        public void AcceptChanges()
        {
            throw new NotImplementedException();
        }
    }
}