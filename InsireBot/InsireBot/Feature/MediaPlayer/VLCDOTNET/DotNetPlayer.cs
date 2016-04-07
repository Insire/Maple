using System;
using System.Diagnostics;
using System.Linq;
using VlcWrapper;
using Vlc.DotNet.Wpf;
using Vlc.DotNet.Core;

namespace InsireBot.MediaPlayer
{
    public sealed class DotNetPlayer : BasePlayer, IMediaPlayer<IMediaItem>
    {
        private VlcMediaPlayer _vlcMediaPlayer;

        public event CompletedMediaItemEventHandler CompletedMediaItem;

        public int VolumeMax { get; }
        public int VolumeMin { get; }

        private bool _disposed;
        public bool Disposed
        {
            get { return _disposed; }
            private set
            {
                _disposed = value;
                RaisePropertyChanged(nameof(Disposed));
            }
        }

        public bool IsPlaying
        {
            get { return _vlcMediaPlayer.IsPlaying; }
        }

        private DotNetPlayerSettings _settings;
        public DotNetPlayerSettings Settings
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

        public bool Silent
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

        public int Volume
        {
            get { return _vlcMediaPlayer.Volume; }
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

        private bool _isShuffling;
        public bool IsShuffling
        {
            get { return _isShuffling; }
            set
            {
                if (_isShuffling != value)
                {
                    _isShuffling = value;
                    RaisePropertyChanged(nameof(IsShuffling));
                }
            }
        }

        public DotNetPlayer(IDataService dataService) : base(dataService)
        {
            VolumeMin = 0;
            VolumeMax = 100;

        }

        public DotNetPlayer(IDataService dataService, DotNetPlayerSettings settings) : this(dataService)
        {
            Settings = settings;

            ValidateSettings();

            AudioDevice = AudioDevices?.FirstOrDefault();

            InitializeProperties();
        }

        public DotNetPlayer(IDataService dataService, DotNetPlayerSettings settings, AudioDevice audioDevice) : this(dataService, settings)
        {
            AudioDevice = audioDevice;

            InitializeProperties();
        }

        private void InitializeProperties()
        {
            if (AudioDevice == null)
                throw new ArgumentNullException(nameof(AudioDevice));

            Settings.Options[1] = string.Format(Settings.Options[1], AudioDevice);
            _vlcMediaPlayer = new VlcMediaPlayer(Settings.VlcLibDirectory, Settings.Options);

            InitializeEvents();
        }

        private void InitializeEvents()
        {
            _vlcMediaPlayer.Buffering += Buffering;
            _vlcMediaPlayer.EncounteredError += EncounteredError;
            _vlcMediaPlayer.EndReached += EndReached;
            _vlcMediaPlayer.Playing += Playing;
            _vlcMediaPlayer.Stopped += Stopped; ;
        }

        private void Playing(VlcControl sender, VlcEventArgs<EventArgs> e)
        {
            Debug.WriteLine("VlcMediaPlayer_Playing");
            RaisePropertyChanged(nameof(IsPlaying));

        }

        private void Stopped(VlcControl sender, VlcEventArgs<EventArgs> e)
        {
            Debug.WriteLine("VlcMediaPlayer_Stopped");
            RaisePropertyChanged(nameof(IsPlaying));
        }

        private void EndReached(VlcControl sender, VlcEventArgs<EventArgs> e)
        {
            CompletedMediaItem?.Invoke(this, new CompletedMediaItemEventEventArgs(Current));
            Debug.WriteLine("VlcMediaPlayer_EndReached");
            RaisePropertyChanged(nameof(IsPlaying));
        }

        private void EncounteredError(VlcControl sender, VlcEventArgs<EventArgs> e)
        {
            Debug.WriteLine("VlcMediaPlayer_EncounteredError");
            foreach (var message in _vlcMediaPlayer.LogMessages)
                Debug.WriteLine($"{message.Message} {message.Header} {message.Name}");

            RaisePropertyChanged(nameof(IsPlaying));
        }

        private void Buffering(VlcControl sender, VlcEventArgs<float> e)
        {
            Debug.WriteLine("Buffering");
            RaisePropertyChanged(nameof(IsPlaying));
        }

        public void ValidateSettings()
        {
            if (Settings == null)
                throw new DotNetPlayerException("DotNetPlayerSettings were null", new ArgumentNullException(nameof(Settings)));

            if (string.IsNullOrEmpty(Settings.Directory))
                throw new DotNetPlayerException("DotNetPlayerSettings.Directory was empty", new ArgumentNullException(nameof(Settings.Directory)));

            if (string.IsNullOrEmpty(Settings.Extension))
                throw new DotNetPlayerException("DotNetPlayerSettings.Extension was empty", new ArgumentNullException(nameof(Settings.Extension)));

            if (string.IsNullOrEmpty(Settings.FileName))
                throw new DotNetPlayerException("DotNetPlayerSettings.FileName was empty", new ArgumentNullException(nameof(Settings.FileName)));

            if (Settings.Options.Any(p => string.IsNullOrEmpty(p)))
                throw new DotNetPlayerException("DotNetPlayerSettings contained an empty string", new ArgumentNullException(nameof(Settings.FileName)));

            if (!Settings.VlcLibDirectory.Exists)
                throw new DotNetPlayerException("VlcLibDirectory in DotNetPlayerSettings doesn't exist");

            //TODO more checks on VlcLibDirectory
        }

        /// <summary>
        /// Pauses playback
        /// </summary>
        public void Pause()
        {
            _vlcMediaPlayer.Pause();

            RaisePropertyChanged(nameof(IsPlaying));
        }

        /// <summary>
        /// Stops playback, clears current MediaItem
        /// </summary>
        public void Stop()
        {
            _vlcMediaPlayer.Stop();
            Current = null;

            RaisePropertyChanged(nameof(Current));
            RaisePropertyChanged(nameof(IsPlaying));
        }

        public void Play(IMediaItem item)
        {
            if (IsPlaying)
                Stop();

            if (item != null)
            {
                _vlcMediaPlayer.Play(new Uri(item.Location));
                Current = item;

                RaisePropertyChanged(nameof(Current));
                RaisePropertyChanged(nameof(IsPlaying));
            }
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
