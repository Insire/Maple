using System;
using System.Linq;
using Vlc.DotNet.Core;

namespace InsireBotCore
{
    public sealed class DotNetPlayer : BasePlayer
    {
        private bool _buffering;
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private VlcMediaPlayer _vlcMediaPlayer;

        public override bool IsPlaying
        {
            get { return _vlcMediaPlayer?.IsPlaying == true; }
        }

        public override bool CanPlay
        {
            get
            {
                return _vlcMediaPlayer != null
                        && !_vlcMediaPlayer.Disposed
                        && (_vlcMediaPlayer.IsPaused || !_vlcMediaPlayer.IsPlaying);
            }
        }

        private ISettings _settings;
        public ISettings Settings
        {
            get { return _settings; }
            internal set { SetValue(ref _settings, value); }
        }

        public override bool Silent
        {
            get { return _vlcMediaPlayer.IsMute; }
            set
            {
                if (_vlcMediaPlayer.IsMute != value)
                {
                    _vlcMediaPlayer.IsMute = value;
                    OnPropertyChanged(nameof(Silent));
                }
            }
        }

        public override int Volume
        {
            get { return _vlcMediaPlayer?.Volume == null ? 0 : _vlcMediaPlayer.Volume; }
            set
            {
                if (_vlcMediaPlayer.Volume != value)
                {
                    var newValue = value;

                    if (newValue > VolumeMax)
                        newValue = VolumeMax;

                    if (newValue < VolumeMin)
                        newValue = VolumeMin;

                    _vlcMediaPlayer.Volume = newValue;

                    OnPropertyChanged(nameof(Volume));
                }
            }
        }

        public DotNetPlayer(IDataService dataService) : base(dataService)
        {
            _buffering = false;
            Playlist = new Playlist<IMediaItem>();
            Playlist.AddRange(dataService.GetCurrentMediaItems());

            Settings = dataService.GetMediaPlayerSettings();
            VolumeMin = 0;
            VolumeMax = 100;

            AudioDevice = AudioDevices?.FirstOrDefault();
        }

        public DotNetPlayer(IDataService dataService, ISettings settings) : this(dataService)
        {
            Settings = settings;

            ValidateSettings();
            InitializeProperties();
        }

        public DotNetPlayer(IDataService dataService, ISettings settings, AudioDevice audioDevice) : this(dataService, settings)
        {
            AudioDevice = audioDevice;

            InitializeProperties();
        }

        private void InitializeProperties()
        {
            _log.Info("initializing VlcMediaPlayer");

            if (AudioDevice == null)
                _log.Error("no suitable AudioDevice found", new ArgumentNullException(nameof(AudioDevice)));

            Settings.Options[1] = string.Format(Settings.Options[1], AudioDevice);
            _vlcMediaPlayer = new VlcMediaPlayer(Settings.Directory, Settings.Options);

            InitializeEvents();
        }

        private void InitializeEvents()
        {
            _vlcMediaPlayer.Buffering += Buffering;
            _vlcMediaPlayer.EncounteredError += EncounteredError;
            _vlcMediaPlayer.EndReached += EndReached;
            _vlcMediaPlayer.Playing += Playing;
            _vlcMediaPlayer.Stopped += Stopped;
        }

        private void Stopped(object sender, VlcMediaPlayerStoppedEventArgs e)
        {
            _log.Info("VlcMediaPlayer_Stopped");
            OnPropertyChanged(nameof(IsPlaying));
        }

        private void Playing(object sender, VlcMediaPlayerPlayingEventArgs e)
        {
            if (_vlcMediaPlayer.IsPlaying)
                _buffering = false; // Playback has begun, so we can uncheck the buffering flag
            _log.Info($"VlcMediaPlayer_Playing ({_vlcMediaPlayer.IsPlaying})");
            OnPropertyChanged(nameof(IsPlaying));
        }

        private void EndReached(object sender, VlcMediaPlayerEndReachedEventArgs e)
        {
            // VLC throws this event, when its still buffering and didnt start playback yet
            if (!_buffering)
                Player_CompletedMediaItem(this, new CompletedMediaItemEventEventArgs(Playlist.CurrentItem));

            _log.Info("VlcMediaPlayer_EndReached");
            OnPropertyChanged(nameof(IsPlaying));
        }

        private void EncounteredError(object sender, VlcMediaPlayerEncounteredErrorEventArgs e)
        {
            _log.Info("VlcMediaPlayer_EncounteredError");
            OnPropertyChanged(nameof(IsPlaying));
        }

        private void Buffering(object sender, VlcMediaPlayerBufferingEventArgs e)
        {
            _log.Info($"Buffering {e.NewCache}");
            OnPropertyChanged(nameof(IsPlaying));
        }

        public void ValidateSettings()
        {
            if (Settings == null)
                _log.Error("DotNetPlayerSettings were null", new ArgumentNullException(nameof(Settings)));

            if (string.IsNullOrEmpty(Settings.FileName))
                _log.Error("DotNetPlayerSettings.FileName was empty", new ArgumentNullException(nameof(Settings.FileName)));

            if (Settings.Options.Any(p => string.IsNullOrEmpty(p)))
                _log.Error("DotNetPlayerSettings contained an empty string", new ArgumentNullException(nameof(Settings.FileName)));

            if (!Settings.Directory.Exists)
                _log.Error("VlcLibDirectory in DotNetPlayerSettings doesn't exist", new DotNetPlayerException("VlcLibDirectory in DotNetPlayerSettings doesn't exist"));

            //TODO more checks on VlcLibDirectory
        }

        /// <summary>
        /// Pauses playback
        /// </summary>
        public override void Pause()
        {
            if (IsPlaying)
            {
                _log.Info($"pausing Playback");
                _vlcMediaPlayer.Pause();
                _log.Info($"paused Playback");

                OnPropertyChanged(nameof(IsPlaying));
            }
        }

        /// <summary>
        /// Stops playback, clears current MediaItem
        /// </summary>
        public override void Stop()
        {
            if (IsPlaying)
            {
                _log.Info($"stopping Playback");
                _vlcMediaPlayer.Stop();
                _log.Info($"stopped Playback");

                OnPropertyChanged(nameof(IsPlaying));
            }
        }

        public override void Play(IMediaItem item)
        {
            if (IsPlaying)
                Stop();

            if (item != null)
            {
                _log.Info($"Playing: {item}");
                _buffering = true;
                Playlist.Set(item);
                _vlcMediaPlayer.Play(new Uri(item.Location));

                OnPropertyChanged(nameof(IsPlaying));
            }
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (_vlcMediaPlayer.IsPlaying)
                Stop();

            if (disposing)
            {
                _vlcMediaPlayer.Buffering -= Buffering; ;
                _vlcMediaPlayer.EncounteredError -= EncounteredError;
                _vlcMediaPlayer.EndReached -= EndReached;
                _vlcMediaPlayer.Playing -= Playing;
                _vlcMediaPlayer.Stopped -= Stopped;

                _vlcMediaPlayer.Dispose();
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            Disposed = true;
        }
    }
}
