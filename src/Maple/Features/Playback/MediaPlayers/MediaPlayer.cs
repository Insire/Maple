using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Maple.Domain;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Commands;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    [DebuggerDisplay("MediaPlayer: {Sequence}, {Name}")]
    public sealed class MediaPlayer : ViewModelBase, IMediaPlayer
    {
        private bool _disposed;

        private int _id;
        public int Id
        {
            get { return _id; }
            private set { SetValue(ref _id, value); }
        }

        private int _sequence;
        public int Sequence
        {
            get { return _sequence; }
            set { SetValue(ref _sequence, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetValue(ref _name, value); }
        }

        private bool _isPrimary;
        public bool IsPrimary
        {
            get { return _isPrimary; }
            set { SetValue(ref _isPrimary, value); }
        }

        private string _createdBy;
        public string CreatedBy
        {
            get { return _createdBy; }
            private set { SetValue(ref _createdBy, value); }
        }

        private string _updatedBy;
        public string UpdatedBy
        {
            get { return _updatedBy; }
            private set { SetValue(ref _updatedBy, value); }
        }

        private DateTime _updatedOn;
        public DateTime UpdatedOn
        {
            get { return _updatedOn; }
            private set { SetValue(ref _updatedOn, value); }
        }

        private DateTime _createdOn;
        public DateTime CreatedOn
        {
            get { return _createdOn; }
            private set { SetValue(ref _createdOn, value); }
        }

        private Playlist _playlist;
        public Playlist Playlist
        {
            get { return _playlist; }
            set
            {
                if (SetValue(ref _playlist, value))
                {
                    PlaylistId = _playlist?.Id;
                }
            }
        }

        private int? _playlistId;
        public int? PlaylistId
        {
            get { return _playlistId; }
            set { SetValue(ref _playlistId, value); }
        }

        private AudioDevice _audioDevice;
        public AudioDevice AudioDevice
        {
            get { return _audioDevice; }
            set
            {
                if (SetValue(ref _audioDevice, value))
                {
                    AudioDeviceId = _audioDevice?.Id;
                }
            }
        }

        private int? _audioDeviceId;
        public int? AudioDeviceId
        {
            get { return _audioDeviceId; }
            set { SetValue(ref _audioDeviceId, value); }
        }

        private IPlaybackService _playback;
        public IPlaybackService Playback
        {
            get { return _playback; }
            private set { SetValue(ref _playback, value); }
        }

        private bool _isDeleted;
        public bool IsDeleted
        {
            get { return _isDeleted; }
            private set { SetValue(ref _isDeleted, value); }
        }

        public ICommand PlayCommand { get; }
        public ICommand PauseCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand PreviousCommand { get; }
        public ICommand StopCommand { get; }

        public bool IsPlaying => Playback.IsPlaying;

        public MediaPlayer(IScarletCommandBuilder commandBuilder, IPlaybackService playbackService)
            : base(commandBuilder)
        {
            Playback = playbackService ?? throw new ArgumentNullException(nameof(playbackService));

            PlayCommand = new RelayCommand<MediaItem>(CommandManager, Play, CanPlay);
            PreviousCommand = new RelayCommand(CommandManager, Previous, () => Playlist?.CanPrevious() == true && CanPrevious());
            NextCommand = new RelayCommand(CommandManager, Next, () => Playlist?.CanNext() == true && CanNext());
            PauseCommand = new RelayCommand(CommandManager, Pause, () => CanPause());
            StopCommand = new RelayCommand(CommandManager, Stop, () => CanStop());
        }

        public MediaPlayer(MediaPlayer mediaPlayer)
            : this(mediaPlayer.CommandBuilder, mediaPlayer.Playback)
        {
            Id = mediaPlayer.Id;
            Name = mediaPlayer.Name;
            Sequence = mediaPlayer.Sequence;

            IsPrimary = mediaPlayer.IsPrimary;
            Playlist = mediaPlayer.Playlist;
            Playback = mediaPlayer.Playback;

            CreatedBy = mediaPlayer.CreatedBy;
            CreatedOn = mediaPlayer.CreatedOn;
            UpdatedBy = mediaPlayer.UpdatedBy;
            UpdatedOn = mediaPlayer.UpdatedOn;
        }

        public MediaPlayer(IScarletCommandBuilder commandBuilder, MediaPlayerModel mediaPlayer, Playlist playlist, IPlaybackService playbackService)
            : this(commandBuilder, playbackService)
        {
            Id = mediaPlayer.Id;
            Name = mediaPlayer.Name;
            Sequence = mediaPlayer.Sequence;

            IsPrimary = mediaPlayer.IsPrimary;

            Playlist = playlist;
            Playback = playbackService;

            CreatedBy = mediaPlayer.CreatedBy;
            CreatedOn = mediaPlayer.CreatedOn;
            UpdatedBy = mediaPlayer.UpdatedBy;
            UpdatedOn = mediaPlayer.UpdatedOn;
        }

        public void Play(MediaItem mediaItem)
        {
            if (mediaItem is null)
                throw new ArgumentNullException(nameof(mediaItem));

            // TODO

            OnPropertyChanged(nameof(IsPlaying));
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
                {
                    await Playlist.Add(item).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public async Task Remove(MediaItem item)
        {
            using (BusyStack.GetToken())
            {
                await Playlist.Remove(item).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Pauses this instance.
        /// </summary>
        public void Pause()
        {
            using (BusyStack.GetToken())
            {
                Playback.Pause();
            }

            OnPropertyChanged(nameof(IsPlaying));
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            using (BusyStack.GetToken())
            {
                Playback.Stop();
            }

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
            return Playback.CanPause();
        }

        /// <summary>
        /// Determines whether this instance can stop.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can stop; otherwise, <c>false</c>.
        /// </returns>
        public bool CanStop()
        {
            return Playback.CanStop();
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
            return Playback.CanPlay(item);
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            if (IsPlaying)
            {
                Stop();
            }

            if (disposing)
            {
                if (Playback != null)
                {
                    Playback?.Dispose();
                    Playback = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}
