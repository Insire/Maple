using System;
using System.Diagnostics;
using System.Linq;
using VlcWrapper;

namespace InsireBot.MediaPlayer
{
    public sealed class DotNetPlayer : BasePlayer, IMediaPlayer<IMediaItem>
    {
        private VlcMediaPlayer _vlcMediaPlayer;

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

                    if (newValue > 100)
                        newValue = 100;

                    if (newValue < 0)
                        newValue = 0;

                    _vlcMediaPlayer.Volume = newValue;

                    RaisePropertyChanged(nameof(Volume));
                }
            }
        }

        public int VolumeMax { get; }

        public int VolumeMin { get; }

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

            Initialize();
        }

        public DotNetPlayer(IDataService dataService, DotNetPlayerSettings settings, AudioDevice audioDevice) : this(dataService, settings)
        {
            AudioDevice = audioDevice;
            Initialize();
        }

        private void Initialize()
        {
            if (AudioDevice == null)
                throw new ArgumentNullException(nameof(AudioDevice));

            Settings.Options[1] = string.Format(Settings.Options[1], AudioDevice);
            _vlcMediaPlayer = new VlcMediaPlayer(Settings.VlcLibDirectory, Settings.Options);

            _vlcMediaPlayer.Buffering += VlcMediaPlayer_Buffering1; ;
            _vlcMediaPlayer.EncounteredError += VlcMediaPlayer_EncounteredError1; ;
            _vlcMediaPlayer.EndReached += VlcMediaPlayer_EndReached1;
            _vlcMediaPlayer.Playing += VlcMediaPlayer_Playing;
            _vlcMediaPlayer.Stopped += VlcMediaPlayer_Stopped1; ;
            _vlcMediaPlayer.TitleChanged += VlcMediaPlayer_TitleChanged1;
        }

        private void VlcMediaPlayer_Playing(Vlc.DotNet.Wpf.VlcControl sender, Vlc.DotNet.Core.VlcEventArgs<EventArgs> e)
        {
            Debug.WriteLine("VlcMediaPlayer_Playing");
        }

        private void VlcMediaPlayer_TitleChanged1(Vlc.DotNet.Wpf.VlcControl sender, Vlc.DotNet.Core.VlcEventArgs<long> e)
        {
            Debug.WriteLine("VlcMediaPlayer_TitleChanged");
        }

        private void VlcMediaPlayer_Stopped1(Vlc.DotNet.Wpf.VlcControl sender, Vlc.DotNet.Core.VlcEventArgs<EventArgs> e)
        {
            Debug.WriteLine("VlcMediaPlayer_Stopped");
        }

        private void VlcMediaPlayer_EndReached1(Vlc.DotNet.Wpf.VlcControl sender, Vlc.DotNet.Core.VlcEventArgs<EventArgs> e)
        {
            Debug.WriteLine("VlcMediaPlayer_EndReached");
        }

        private void VlcMediaPlayer_EncounteredError1(Vlc.DotNet.Wpf.VlcControl sender, Vlc.DotNet.Core.VlcEventArgs<EventArgs> e)
        {
            Debug.WriteLine("VlcMediaPlayer_EncounteredError");
            foreach ( var message in _vlcMediaPlayer.LogMessages)
                Debug.WriteLine($"{message.Message} {message.Header} {message.Name}");
        }

        private void VlcMediaPlayer_Buffering1(Vlc.DotNet.Wpf.VlcControl sender, Vlc.DotNet.Core.VlcEventArgs<float> e)
        {
            Debug.WriteLine("Buffering");
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

        public override void Pause()
        {
            _vlcMediaPlayer.Pause();
        }

        public override void Stop()
        {
            _vlcMediaPlayer.Stop();
        }

        public override void Play(IMediaItem item)
        {
            if (item != null)
            {
                _vlcMediaPlayer.Play(new Uri(item.Location));
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
