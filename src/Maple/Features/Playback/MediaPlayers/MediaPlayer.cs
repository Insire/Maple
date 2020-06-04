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
    [DebuggerDisplay("{Name}, {Sequence}")]
    public sealed class MediaPlayer : ViewModelBase, ISequence
    {
        private bool _disposed;

        public bool IsPlaying => Player.IsPlaying;

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

        private Playlist _playlist;
        public Playlist Playlist
        {
            get { return _playlist; }
            set { SetValue(ref _playlist, value); }
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
            set { SetValue(ref _createdBy, value); }
        }

        private string _updatedBy;
        public string UpdatedBy
        {
            get { return _updatedBy; }
            set { SetValue(ref _updatedBy, value); }
        }

        private DateTime _updatedOn;
        public DateTime UpdatedOn
        {
            get { return _updatedOn; }
            set { SetValue(ref _updatedOn, value); }
        }

        private DateTime _createdOn;
        public DateTime CreatedOn
        {
            get { return _createdOn; }
            set { SetValue(ref _createdOn, value); }
        }

        public MediaPlayer(IScarletCommandBuilder commandBuilder, IMediaPlayer player)
            : base(commandBuilder)
        {
            Player = player ?? throw new ArgumentNullException(nameof(player));

            PlayCommand = new RelayCommand<MediaItem>(CommandManager, Play, CanPlay);
            PreviousCommand = new RelayCommand(CommandManager, Previous, () => Playlist?.CanPrevious() == true && CanPrevious());
            NextCommand = new RelayCommand(CommandManager, Next, () => Playlist?.CanNext() == true && CanNext());
            PauseCommand = new RelayCommand(CommandManager, Pause, () => CanPause());
            StopCommand = new RelayCommand(CommandManager, Stop, () => CanStop());
        }

        public void Play(MediaItem mediaItem)
        {
            if (mediaItem == null)
                throw new ArgumentNullException(nameof(mediaItem));

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
                    await Playlist.Add(item).ConfigureAwait(false);
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
            if (_disposed)
            {
                return;
            }
            _disposed = true;

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
    }
}
