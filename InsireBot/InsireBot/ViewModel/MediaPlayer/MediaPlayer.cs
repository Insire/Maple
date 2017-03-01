using Maple.Core;
using Maple.Data;
using Maple.Localization.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Maple
{
    public class MediaPlayer : BaseViewModel<Data.MediaPlayer>, IDisposable
    {
        private readonly IDbContext _context;
        private readonly ITranslationManager _manager;

        public bool IsPlaying { get { return Player.IsPlaying; } }
        public bool Disposed { get; private set; }

        private IMediaPlayer _player;
        public IMediaPlayer Player
        {
            get { return _player; }
            private set { SetValue(ref _player, value); }
        }

        public ICommand PlayCommand { get; private set; }
        public ICommand PauseCommand { get; private set; }
        public ICommand NextCommand { get; private set; }
        public ICommand PreviousCommand { get; private set; }
        public ICommand StopCommand { get; private set; }
        public ICommand LoadFromFileCommand { get; private set; }
        public ICommand LoadFromFolderCommand { get; private set; }
        public ICommand LoadFromUrlCommand { get; private set; }
        public ICommand RemoveRangeCommand { get; protected set; }
        public ICommand RemoveCommand { get; protected set; }
        public ICommand ClearCommand { get; protected set; }

        private AudioDevices _audioDevices;
        public AudioDevices AudioDevices
        {
            get { return _audioDevices; }
            private set { SetValue(ref _audioDevices, value); }
        }

        private Playlist _playlist;
        public Playlist Playlist
        {
            get { return _playlist; }
            set { SetValue(ref _playlist, value, Changing: OnPlaylistChanging, Changed: OnPlaylistChanged); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetValue(ref _name, value, Changed: () => Model.Name = value); }
        }

        private bool _isPrimary;
        public bool IsPrimary
        {
            get { return _isPrimary; }
            protected set { SetValue(ref _isPrimary, value, Changed: () => Model.IsPrimary = value); }
        }

        public MediaPlayer(ITranslationManager manager, IMediaPlayer player, Data.MediaPlayer model) : base(model)
        {
            _manager = manager ?? throw new ArgumentNullException(nameof(manager), $"{nameof(manager)} {Resources.IsRequired}");
            Player = player ?? throw new ArgumentNullException(nameof(player), $"{nameof(player)} {Resources.IsRequired}");

            Name = model.Name;
            AudioDevices = new AudioDevices();

            Player.PlayingMediaItem += Player_PlayingMediaItem;
            Player.CompletedMediaItem += MediaPlayer_CompletedMediaItem;
            Player.AudioDeviceChanged += Player_AudioDeviceChanged;
            Player.AudioDeviceChanging += Player_AudioDeviceChanging;

            var device = AudioDevices.Items.FirstOrDefault(p => p.Name == Model.DeviceName);
            if (device != null)
                Player.AudioDevice = device;

            InitiliazeCommands();
        }

        //public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (string.IsNullOrWhiteSpace(Name))
        //        yield return new ValidationResult($"{nameof(Name)} {Resources.IsRequired}", new[] { nameof(Name) });

        //    if (Playlist == null)
        //        yield return new ValidationResult($"{nameof(Playlist)} {Resources.IsRequired}", new[] { nameof(Playlist) });

        //    if (Player == null)
        //        yield return new ValidationResult($"{nameof(Player)} {Resources.IsRequired}", new[] { nameof(Player) });

        //    if (Player?.AudioDevice == null)
        //        yield return new ValidationResult($"{nameof(Player.AudioDevice)} {Resources.IsRequired}", new[] { nameof(Player.AudioDevice) });
        //}

        private void Player_AudioDeviceChanging(object sender, EventArgs e)
        {
            // TODO
        }

        private void Player_AudioDeviceChanged(object sender, AudioDeviceChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e?.AudioDevice?.Name))
                Model.DeviceName = e.AudioDevice.Name;
        }

        private void OnPlaylistChanging()
        {
            Stop();
        }

        private void OnPlaylistChanged()
        {
            Model.PlaylistId = Playlist.Id;
            // TODO: maybe add optional endless playback

            UpdatePlaylistCommands();
        }

        private void UpdatePlaylistCommands()
        {
            if (Playlist != null)
            {
                LoadFromFileCommand = Playlist.LoadFromFileCommand;
                LoadFromFolderCommand = Playlist.LoadFromFolderCommand;
                LoadFromUrlCommand = Playlist.LoadFromUrlCommand;
            }
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
            PreviousCommand = new RelayCommand(Previous, () => Playlist?.CanPrevious() == true && CanPrevious());
            NextCommand = new RelayCommand(Next, () => Playlist?.CanNext() == true && CanNext());
            PauseCommand = new RelayCommand(Pause, () => CanPause());
            StopCommand = new RelayCommand(Stop, () => CanStop());
            RemoveCommand = new RelayCommand<MediaItemViewModel>(Remove, CanRemove);
            ClearCommand = new RelayCommand(Clear, CanClear);

            UpdatePlaylistCommands();
        }

        public void Clear()
        {
            using (_busyStack.GetToken())
                Playlist.Clear();
        }

        public bool CanClear()
        {
            return !IsBusy && Playlist.ItemCount > 0;
        }

        public void AddRange(IEnumerable<MediaItemViewModel> mediaItems)
        {
            using (_busyStack.GetToken())
            {
                foreach (var item in mediaItems)
                    Playlist.Add(item);
            }
        }

        public void Add(MediaItemViewModel mediaItem)
        {
            using (_busyStack.GetToken())
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
        }

        public void Remove(MediaItemViewModel item)
        {
            using (_busyStack.GetToken())
                Playlist.Remove(item);
        }

        private bool CanRemove(MediaItemViewModel item)
        {
            using (_busyStack.GetToken())
                return Playlist.CanRemove(item);
        }

        public void Pause()
        {
            using (_busyStack.GetToken())
                Player.Pause();
        }

        public void Stop()
        {
            using (_busyStack.GetToken())
                Player.Stop();
        }

        public void Previous()
        {
            using (_busyStack.GetToken())
            {
                var item = Playlist.Previous();
                Player.Play(item);
            }
        }

        public void Next()
        {
            using (_busyStack.GetToken())
            {
                var item = Playlist.Next();
                Player.Play(item);
            }
        }

        public bool CanNext()
        {
            var item = Playlist.Next();
            return CanPlay(item);
        }

        public bool CanPrevious()
        {
            var item = Playlist.Previous();
            return CanPlay(item);
        }

        public bool CanPause()
        {
            return Player.CanPause();
        }

        public bool CanStop()
        {
            return Player.CanStop();
        }

        private bool CanPlay(MediaItemViewModel item)
        {
            return Player.CanPlay(item);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (IsPlaying)
                Stop();

            if (disposing)
            {
                Player.PlayingMediaItem -= Player_PlayingMediaItem;
                Player.CompletedMediaItem -= MediaPlayer_CompletedMediaItem;
                Player.AudioDeviceChanged -= Player_AudioDeviceChanged;
                Player.AudioDeviceChanging -= Player_AudioDeviceChanging;

                Player?.Dispose();
                Player = null;

                // Free any other managed objects here.
            }

            // Free any unmanaged objects here.
            Disposed = true;
        }
    }
}