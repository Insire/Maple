using System;
using System.Diagnostics;
using System.Linq;
using Vlc.DotNet.Core;

namespace InsireBotCore
{
    public sealed class DotNetPlayer : BasePlayer
    {
        private VlcMediaPlayer _vlcMediaPlayer;

        public override bool IsPlaying
        {
            get { return _vlcMediaPlayer?.IsPlaying == true; }
        }

        private ISettings _settings;
        public ISettings Settings
        {
            get { return _settings; }
            internal set
            {
                if (_settings != value && value != null)
                {
                    _settings = value;
                    RaisePropertyChanged(nameof(Settings));
                }
            }
        }

        public override bool Silent
        {
            get { return _vlcMediaPlayer.IsMute; }
            set
            {
                if (_vlcMediaPlayer.IsMute != value)
                {
                    _vlcMediaPlayer.IsMute = value;
                    RaisePropertyChanged(nameof(Silent));
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

                    RaisePropertyChanged(nameof(Volume));
                }
            }
        }

        public DotNetPlayer(IDataService dataService) : base(dataService)
        {
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
            if (AudioDevice == null)
                throw new ArgumentNullException(nameof(AudioDevice));

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
            Debug.WriteLine("VlcMediaPlayer_Stopped");
            RaisePropertyChanged(nameof(IsPlaying));
        }

        private void Playing(object sender, VlcMediaPlayerPlayingEventArgs e)
        {
            Debug.WriteLine("VlcMediaPlayer_Playing");
            RaisePropertyChanged(nameof(IsPlaying));
        }

        private void EndReached(object sender, VlcMediaPlayerEndReachedEventArgs e)
        {
            Player_CompletedMediaItem(this, new CompletedMediaItemEventEventArgs(Playlist.CurrentItem));
            RaisePropertyChanged(nameof(IsPlaying));
        }

        private void EncounteredError(object sender, VlcMediaPlayerEncounteredErrorEventArgs e)
        {
            Debug.WriteLine("VlcMediaPlayer_EncounteredError");
            RaisePropertyChanged(nameof(IsPlaying));
        }

        private void Buffering(object sender, Vlc.DotNet.Core.VlcMediaPlayerBufferingEventArgs e)
        {
            Debug.WriteLine("Buffering");
            RaisePropertyChanged(nameof(IsPlaying));
        }

        public void ValidateSettings()
        {
            if (Settings == null)
                throw new DotNetPlayerException("DotNetPlayerSettings were null", new ArgumentNullException(nameof(Settings)));

            if (string.IsNullOrEmpty(Settings.FileName))
                throw new DotNetPlayerException("DotNetPlayerSettings.FileName was empty", new ArgumentNullException(nameof(Settings.FileName)));

            if (Settings.Options.Any(p => string.IsNullOrEmpty(p)))
                throw new DotNetPlayerException("DotNetPlayerSettings contained an empty string", new ArgumentNullException(nameof(Settings.FileName)));

            if (!Settings.Directory.Exists)
                throw new DotNetPlayerException("VlcLibDirectory in DotNetPlayerSettings doesn't exist");

            //TODO more checks on VlcLibDirectory
        }

        /// <summary>
        /// Pauses playback
        /// </summary>
        public override void Pause()
        {
            _vlcMediaPlayer.Pause();

            RaisePropertyChanged(nameof(IsPlaying));
        }

        /// <summary>
        /// Stops playback, clears current MediaItem
        /// </summary>
        public override void Stop()
        {
            _vlcMediaPlayer.Stop();

            RaisePropertyChanged(nameof(IsPlaying));
        }

        public override void Play(IMediaItem item)
        {
            if (IsPlaying)
                Stop();

            if (item != null)
            {
                _vlcMediaPlayer.Play(new Uri(item.Location));

                RaisePropertyChanged(nameof(IsPlaying));
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
