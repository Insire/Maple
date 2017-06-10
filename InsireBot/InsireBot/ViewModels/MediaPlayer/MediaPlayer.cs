using FluentValidation;
using Maple.Core;
using Maple.Localization.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace Maple
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Maple.Core.BaseViewModel{Maple.Data.MediaPlayer}" />
    /// <seealso cref="System.IDisposable" />
    /// <seealso cref="Maple.Core.IChangeState" />
    [DebuggerDisplay("{Name}, {Sequence}")]
    public class MediaPlayer : ValidableBaseDataViewModel<MediaPlayer, Data.MediaPlayer>, IDisposable, IChangeState, ISequence
    {
        protected readonly ILocalizationService _manager;

        /// <summary>
        /// Gets a value indicating whether this instance is playing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is playing; otherwise, <c>false</c>.
        /// </value>
        public bool IsPlaying { get { return Player.IsPlaying; } }
        /// <summary>
        /// Gets a value indicating whether this <see cref="MediaPlayer"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }
        /// <summary>
        /// Gets a value indicating whether this instance is new.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is new; otherwise, <c>false</c>.
        /// </value>
        public bool IsNew => Model.IsNew;
        /// <summary>
        /// Gets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeleted => Model.IsDeleted;

        private IMediaPlayer _player;
        /// <summary>
        /// Gets the player.
        /// </summary>
        /// <value>
        /// The player.
        /// </value>
        public IMediaPlayer Player
        {
            get { return _player; }
            private set { SetValue(ref _player, value); }
        }

        /// <summary>
        /// Gets the play command.
        /// </summary>
        /// <value>
        /// The play command.
        /// </value>
        public ICommand PlayCommand { get; private set; }
        /// <summary>
        /// Gets the pause command.
        /// </summary>
        /// <value>
        /// The pause command.
        /// </value>
        public ICommand PauseCommand { get; private set; }
        /// <summary>
        /// Gets the next command.
        /// </summary>
        /// <value>
        /// The next command.
        /// </value>
        public ICommand NextCommand { get; private set; }
        /// <summary>
        /// Gets the previous command.
        /// </summary>
        /// <value>
        /// The previous command.
        /// </value>
        public ICommand PreviousCommand { get; private set; }
        /// <summary>
        /// Gets the stop command.
        /// </summary>
        /// <value>
        /// The stop command.
        /// </value>
        public ICommand StopCommand { get; private set; }
        /// <summary>
        /// Gets the load from file command.
        /// </summary>
        /// <value>
        /// The load from file command.
        /// </value>
        public ICommand LoadFromFileCommand { get; private set; }
        /// <summary>
        /// Gets the load from folder command.
        /// </summary>
        /// <value>
        /// The load from folder command.
        /// </value>
        public ICommand LoadFromFolderCommand { get; private set; }
        /// <summary>
        /// Gets the load from URL command.
        /// </summary>
        /// <value>
        /// The load from URL command.
        /// </value>
        public ICommand LoadFromUrlCommand { get; private set; }
        /// <summary>
        /// Gets or sets the remove range command.
        /// </summary>
        /// <value>
        /// The remove range command.
        /// </value>
        public ICommand RemoveRangeCommand { get; protected set; }
        /// <summary>
        /// Gets or sets the remove command.
        /// </summary>
        /// <value>
        /// The remove command.
        /// </value>
        public ICommand RemoveCommand { get; protected set; }
        /// <summary>
        /// Gets or sets the clear command.
        /// </summary>
        /// <value>
        /// The clear command.
        /// </value>
        public ICommand ClearCommand { get; protected set; }

        private AudioDevices _audioDevices;
        /// <summary>
        /// Gets the audio devices.
        /// </summary>
        /// <value>
        /// The audio devices.
        /// </value>
        public AudioDevices AudioDevices
        {
            get { return _audioDevices; }
            private set { SetValue(ref _audioDevices, value); }
        }

        private Playlist _playlist;
        /// <summary>
        /// Gets or sets the playlist.
        /// </summary>
        /// <value>
        /// The playlist.
        /// </value>
        public Playlist Playlist
        {
            get { return _playlist; }
            set { SetValue(ref _playlist, value, OnChanging: OnPlaylistChanging, OnChanged: OnPlaylistChanged); }
        }

        private int _sequence;
        /// <summary>
        /// the index of this item if its part of a collection
        /// </summary>
        /// <value>
        /// The sequence.
        /// </value>
        public int Sequence
        {
            get { return _sequence; }
            set { SetValue(ref _sequence, value, OnChanged: () => Model.Sequence = value); }
        }

        private string _name;
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return _name; }
            set { SetValue(ref _name, value, OnChanged: () => Model.Name = value); }
        }

        private bool _isPrimary;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is primary.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is primary; otherwise, <c>false</c>.
        /// </value>
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
            get { return _updatedOn; }
            set { SetValue(ref _updatedOn, value, OnChanged: () => Model.CreatedOn = value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaPlayer"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="player">The player.</param>
        /// <param name="model">The model.</param>
        /// <param name="playlist">The playlist.</param>
        /// <param name="devices">The devices.</param>
        /// <exception cref="System.ArgumentNullException">
        /// manager - manager
        /// or
        /// player - player
        /// or
        /// playlist - playlist
        /// </exception>
        public MediaPlayer(ILocalizationService manager, IMediaPlayer player, IValidator<MediaPlayer> validator, AudioDevices devices, Playlist playlist, Data.MediaPlayer model)
            : base(model, validator)
        {
            _manager = manager ?? throw new ArgumentNullException(nameof(manager), $"{nameof(manager)} {Resources.IsRequired}");
            Player = player ?? throw new ArgumentNullException(nameof(player), $"{nameof(player)} {Resources.IsRequired}");
            Playlist = playlist ?? throw new ArgumentNullException(nameof(playlist), $"{nameof(playlist)} {Resources.IsRequired}");

            _name = model.Name;
            _playlist = playlist;
            _audioDevices = devices;
            _sequence = model.Sequence;

            _createdBy = model.CreatedBy;
            _createdOn = model.CreatedOn;
            _updatedBy = model.UpdatedBy;
            _updatedOn = model.UpdatedOn;

            if (AudioDevices.Items.Count > 0)
                Player.AudioDevice = AudioDevices.Items.FirstOrDefault(p => p.Name == Model.DeviceName) ?? AudioDevices.Items[0];

            InitializeSubscriptions();
            InitiliazeCommands();

            Validate();
        }

        private void InitializeSubscriptions()
        {
            Player.PlayingMediaItem += Player_PlayingMediaItem;
            Player.CompletedMediaItem += MediaPlayer_CompletedMediaItem;
            Player.AudioDeviceChanged += Player_AudioDeviceChanged;
            Player.AudioDeviceChanging += Player_AudioDeviceChanging;
        }

        private void InitiliazeCommands()
        {
            PlayCommand = new RelayCommand<MediaItem>(Player.Play, CanPlay);
            PreviousCommand = new RelayCommand(Previous, () => Playlist?.CanPrevious() == true && CanPrevious());
            NextCommand = new RelayCommand(Next, () => Playlist?.CanNext() == true && CanNext());
            PauseCommand = new RelayCommand(Pause, () => CanPause());
            StopCommand = new RelayCommand(Stop, () => CanStop());
            RemoveCommand = new RelayCommand<MediaItem>(Remove, CanRemove);
            ClearCommand = new RelayCommand(Clear, CanClear);

            UpdatePlaylistCommands();
        }

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

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            using (_busyStack.GetToken())
                Playlist.Clear();
        }

        /// <summary>
        /// Determines whether this instance can clear.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can clear; otherwise, <c>false</c>.
        /// </returns>
        public bool CanClear()
        {
            return !IsBusy && Playlist.Count > 0;
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="mediaItems">The media items.</param>
        public void AddRange(IEnumerable<MediaItem> mediaItems)
        {
            using (_busyStack.GetToken())
            {
                foreach (var item in mediaItems)
                    Playlist.Add(item);
            }
        }

        /// <summary>
        /// Adds the specified media item.
        /// </summary>
        /// <param name="mediaItem">The media item.</param>
        public void Add(MediaItem mediaItem)
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
                    mediaItem.Sequence = 0;

                Playlist.Items.Add(mediaItem);
            }
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Remove(MediaItem item)
        {
            using (_busyStack.GetToken())
                Playlist.Remove(item);
        }

        private bool CanRemove(MediaItem item)
        {
            using (_busyStack.GetToken())
                return Playlist.CanRemove(item);
        }

        /// <summary>
        /// Pauses this instance.
        /// </summary>
        public void Pause()
        {
            using (_busyStack.GetToken())
                Player.Pause();
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            using (_busyStack.GetToken())
                Player.Stop();
        }

        /// <summary>
        /// Previouses this instance.
        /// </summary>
        public void Previous()
        {
            using (_busyStack.GetToken())
            {
                var item = Playlist.Previous();
                Player.Play(item);
            }
        }

        /// <summary>
        /// Nexts this instance.
        /// </summary>
        public void Next()
        {
            using (_busyStack.GetToken())
            {
                var item = Playlist.Next();
                Player.Play(item);
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

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
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
