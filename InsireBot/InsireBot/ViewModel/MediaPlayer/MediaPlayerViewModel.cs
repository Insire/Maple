using InsireBot.Core;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System;
using InsireBot.Data;

namespace InsireBot
{
    public class MediaPlayerViewModel : ObservableObject, IValidatableTrackingObject
    {
        public MediaPlayer Model { get; private set; }

        private IMediaPlayer _player;
        public IMediaPlayer Player
        {
            get { return _player; }
            private set { SetValue(ref _player, value); }
        }

        public bool IsPlaying { get { return Player.IsPlaying; } }

        public ICommand PlayCommand { get; private set; }
        public ICommand PauseCommand { get; private set; }
        public ICommand NextCommand { get; private set; }
        public ICommand PreviousCommand { get; private set; }

        private AudioDevices _audioDevices;
        public AudioDevices AudioDevices
        {
            get { return _audioDevices; }
            private set { SetValue(ref _audioDevices, value); }
        }

        private PlaylistViewModel _playlist;
        public PlaylistViewModel Playlist
        {
            get { return _playlist; }
            set { SetValue(ref _playlist, value, Changing: OnPlaylistChanging, Changed: OnPlaylistChanged); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetValue(ref _name, value); }
        }

        public bool IsValid { get; }

        public bool IsChanged { get; }

        public MediaPlayerViewModel(IMediaPlayer player, MediaPlayer mediaPlayer) : base()
        {
            Model = mediaPlayer;
            AudioDevices = new AudioDevices();

            Player = player;
            Player.PlayingMediaItem += Player_PlayingMediaItem;
            Player.CompletedMediaItem += MediaPlayer_CompletedMediaItem;
            Player.AudioDeviceChanged += Player_AudioDeviceChanged;
            Player.AudioDeviceChanging += Player_AudioDeviceChanging;

            InitiliazeCommands();
        }

        private void Player_AudioDeviceChanging(object sender, EventArgs e)
        {

        }

        private void Player_AudioDeviceChanged(object sender, AudioDeviceChangedEventArgs e)
        {
            Model.DeviceName = e.AudioDevice.Name;
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

        public void AddRange(IEnumerable<MediaItemViewModel> mediaItems)
        {
            foreach (var item in mediaItems)
                Playlist.Add(item);
        }

        public void Add(MediaItemViewModel mediaItem)
        {
            if (Playlist.Items.Any())
            {
                var maxIndex = Playlist.Items.Max(p => p.Sequence) + 1;
                if (maxIndex < 0)
                    maxIndex = 0;

                mediaItem.Sequence = maxIndex;
            }
            else
            {
                mediaItem.Sequence = 0;
            }

            Playlist.Items.Add(mediaItem);
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