using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using FluentValidation;
using Maple.Core;
using Maple.Domain;
using Maple.Localization.Properties;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Commands;

namespace Maple
{
    [DebuggerDisplay("{Name}, {Sequence}")]
    public class MediaPlayer : MapleDomainViewModelBase<MediaPlayer, MediaPlayerModel>, ISequence, IChangeState
    {
        public bool IsPlaying => Player.IsPlaying;

        public bool IsNew => Model.IsNew;
        public bool IsDeleted => Model.IsDeleted;

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

        private ICommand _loadFromFileCommand;
        public ICommand LoadFromFileCommand
        {
            get { return _loadFromFileCommand; }
            private set { SetValue(ref _loadFromFileCommand, value); }
        }

        private ICommand _loadFromFolderCommand;
        public ICommand LoadFromFolderCommand
        {
            get { return _loadFromFolderCommand; }
            private set { SetValue(ref _loadFromFolderCommand, value); }
        }

        private ICommand _loadFromUrlCommand;
        public ICommand LoadFromUrlCommand
        {
            get { return _loadFromUrlCommand; }
            private set { SetValue(ref _loadFromUrlCommand, value); }
        }

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
            set { SetValue(ref _playlist, value, OnChanging: OnPlaylistChanging, OnChanged: OnPlaylistChanged); }
        }

        private int _sequence;
        public int Sequence
        {
            get { return _sequence; }
            set { SetValue(ref _sequence, value, OnChanged: () => Model.Sequence = value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetValue(ref _name, value, OnChanged: () => Model.Name = value); }
        }

        private bool _isPrimary;
        public bool IsPrimary
        {
            get { return _isPrimary; }
            protected set { SetValue(ref _isPrimary, value, OnChanged: () => Model.IsPrimary = value); }
        }

        private string _createdBy;
        public string CreatedBy
        {
            get { return _createdBy; }
            set { SetValue(ref _createdBy, value, OnChanged: () => Model.CreatedBy = value); }
        }

        private string _updatedBy;
        public string UpdatedBy
        {
            get { return _updatedBy; }
            set { SetValue(ref _updatedBy, value, OnChanged: () => Model.UpdatedBy = value); }
        }

        private DateTime _updatedOn;
        public DateTime UpdatedOn
        {
            get { return _updatedOn; }
            set { SetValue(ref _updatedOn, value, OnChanged: () => Model.UpdatedOn = value); }
        }

        private DateTime _createdOn;
        public DateTime CreatedOn
        {
            get { return _createdOn; }
            set { SetValue(ref _createdOn, value, OnChanged: () => Model.CreatedOn = value); }
        }

        public MediaPlayer(IMapleCommandBuilder commandBuilder, IMediaPlayer player, IValidator<MediaPlayer> validator, AudioDevices devices, Playlist playlist, MediaPlayerModel model)
            : base(commandBuilder, validator)
        {
            Player = player ?? throw new ArgumentNullException(nameof(player));

            _name = model.Name;
            _audioDevices = devices;
            _sequence = model.Sequence;

            _createdBy = model.CreatedBy;
            _createdOn = model.CreatedOn;
            _updatedBy = model.UpdatedBy;
            _updatedOn = model.UpdatedOn;

            if (AudioDevices.Items.Count > 0)
                Player.AudioDevice = AudioDevices.Items.FirstOrDefault(p => p.Name == Model.DeviceName) ?? AudioDevices[0];

            Playlist = playlist;

            PlayCommand = new RelayCommand<MediaItem>(Play, CanPlay);
            PreviousCommand = new RelayCommand(Previous, () => Playlist?.CanPrevious() == true && CanPrevious());
            NextCommand = new RelayCommand(Next, () => Playlist?.CanNext() == true && CanNext());
            PauseCommand = new RelayCommand(Pause, () => CanPause());
            StopCommand = new RelayCommand(Stop, () => CanStop());

            UpdatePlaylistCommands();
        }

        public void Play(MediaItem mediaItem)
        {
            if (mediaItem == null)
                throw new ArgumentNullException(nameof(mediaItem), $"{nameof(mediaItem)} {Resources.IsRequired}");

            if (!Playlist.MediaItems.Items.Contains(mediaItem))
                throw new ArgumentException("Cant play an item thats not part of the playlist"); // TODO localize

            if (Player.Play(mediaItem))
                Messenger.Publish(new PlayingMediaItemMessage(this, mediaItem, Playlist.Id));

            OnPropertyChanged(nameof(IsPlaying));
        }

        private bool IsSenderEqualsPlayer(object sender)
        {
            if (sender is ScarletMessageBase message)
            {
                return ReferenceEquals(message.Sender, this);
            }

            return false;
        }

        private void Player_AudioDeviceChanging(ViewModelSelectionChangingMessage<AudioDevice> e)
        {
            // TODO handle Player_AudioDeviceChanging
        }

        private void Player_AudioDeviceChanged(ViewModelSelectionChangingMessage<AudioDevice> e)
        {
            if (!string.IsNullOrEmpty(e?.Content?.Name))
                Model.DeviceName = e.Content.Name;
        }

        private void OnPlaylistChanging()
        {
            Stop();
        }

        private void OnPlaylistChanged()
        {
            Model.Playlist = Playlist.Model;
            // TODO: maybe add optional endless playback

            OnPropertyChanged(nameof(Playlist.View));

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

        private void Player_PlayingMediaItem(PlayingMediaItemMessage e)
        {
            // TODO: sync state to other viewmodels
            OnPropertyChanged(nameof(IsPlaying));
        }

        private void MediaPlayer_CompletedMediaItem(CompletedMediaItemMessage e)
        {
            OnPropertyChanged(nameof(IsPlaying));

            Next();
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="mediaItems">The media items.</param>
        public async Task AddRange(IEnumerable<MediaItem> mediaItems)
        {
            using (BusyStack.GetToken())
            {
                foreach (var item in mediaItems)
                    await Playlist.Add(item).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Adds the specified media item.
        /// </summary>
        /// <param name="mediaItem">The media item.</param>
        public async Task Add(MediaItem mediaItem)
        {
            using (BusyStack.GetToken())
            {
                if (Playlist.MediaItems.Items.Any())
                {
                    var maxIndex = Playlist.MediaItems.Items.Max(p => p.Sequence) + 1;
                    if (maxIndex < 0)
                        maxIndex = 0;

                    mediaItem.Sequence = maxIndex;
                }
                else
                    mediaItem.Sequence = 0;

                await Playlist.Add(mediaItem).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public async Task Remove(MediaItem item)
        {
            using (BusyStack.GetToken())
                await Playlist.Remove(item).ConfigureAwait(false);
        }

        /// <summary>
        /// Pauses this instance.
        /// </summary>
        public void Pause()
        {
            using (BusyStack.GetToken())
                Player.Pause();

            OnPropertyChanged(nameof(IsPlaying));
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            using (BusyStack.GetToken())
                Player.Stop();

            OnPropertyChanged(nameof(IsPlaying));
        }

        /// <summary>
        /// Previouses this instance.
        /// </summary>
        public void Previous()
        {
            using (BusyStack.GetToken())
            {
                var item = Playlist.Previous();
                Play(item);
            }
        }

        /// <summary>
        /// Nexts this instance.
        /// </summary>
        public void Next()
        {
            using (BusyStack.GetToken())
            {
                var item = Playlist.Next();
                Play(item);
            }
        }

        /// <summary>
        /// Determines whether this instance can next.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can next; otherwise, <c>false</c>.
        /// </returns>
        public bool CanNext()
        {
            var item = Playlist.Next();
            return CanPlay(item);
        }

        /// <summary>
        /// Determines whether this instance can previous.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can previous; otherwise, <c>false</c>.
        /// </returns>
        public bool CanPrevious()
        {
            var item = Playlist.Previous();
            return CanPlay(item);
        }

        /// <summary>
        /// Determines whether this instance can pause.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can pause; otherwise, <c>false</c>.
        /// </returns>
        public bool CanPause()
        {
            return Player.CanPause();
        }

        /// <summary>
        /// Determines whether this instance can stop.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can stop; otherwise, <c>false</c>.
        /// </returns>
        public bool CanStop()
        {
            return Player.CanStop();
        }

        /// <summary>
        /// Determines whether this instance can play the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if this instance can play the specified item; otherwise, <c>false</c>.
        /// </returns>
        private bool CanPlay(MediaItem item)
        {
            return Player.CanPlay(item);
        }

        protected override void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            if (IsPlaying)
                Stop();

            if (disposing)
            {
                if (Player != null)
                {
                    Player?.Dispose();
                    Player = null;
                }
            }

            base.Dispose(disposing);
        }

        protected override Task RefreshInternal(CancellationToken token)
        {
            ClearSubscriptions();

            Add(Messenger.Subscribe<PlayingMediaItemMessage>(Player_PlayingMediaItem, IsSenderEqualsPlayer));
            Add(Messenger.Subscribe<CompletedMediaItemMessage>(MediaPlayer_CompletedMediaItem, IsSenderEqualsPlayer));
            Add(Messenger.Subscribe<ViewModelSelectionChangingMessage<AudioDevice>>(Player_AudioDeviceChanging, IsSenderEqualsPlayer));
            Add(Messenger.Subscribe<ViewModelSelectionChangingMessage<AudioDevice>>(Player_AudioDeviceChanged, IsSenderEqualsPlayer));

            return Task.CompletedTask;
        }

        protected override Task UnloadInternal(CancellationToken token)
        {
            ClearSubscriptions();

            return Task.CompletedTask;
        }
    }
}
