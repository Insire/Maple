using System;
using System.Linq;
using Vlc.DotNet.Core;

namespace InsireBot.MediaPlayer
{
    public sealed class DotNetPlayer : BasePlayer, IMediaPlayer<IMediaItem>
    {
        private VlcMediaPlayer vlcMediaPlayer;

        private AudioDevice _audioDevice;
        public override AudioDevice AudioDevice
        {
            get { return _audioDevice; }
            set
            {
                if (_audioDevice != value && value != null)
                {
                    _audioDevice = value;
                    RaisePropertyChanged(nameof(AudioDevice));
                }
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
            get { return vlcMediaPlayer.Audio.IsMute; }
            set
            {
                if (vlcMediaPlayer.Audio.IsMute != value)
                {
                    vlcMediaPlayer.Audio.IsMute = value;
                    RaisePropertyChanged(nameof(Silent));
                }
            }
        }

        public int Volume
        {
            get { return vlcMediaPlayer.Audio.Volume; }
            set
            {
                if (vlcMediaPlayer.Audio.Volume != value)
                {
                    var newValue = value;

                    if (newValue > 100)
                        newValue = 100;

                    if (newValue < 0)
                        newValue = 0;

                    vlcMediaPlayer.Audio.Volume = newValue;

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
            vlcMediaPlayer = new VlcMediaPlayer(Settings.VlcLibDirectory, Settings.Options);
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

        public override void Next()
        {
            vlcMediaPlayer.OnMediaPlayerForward();
        }

        public override void Previous()
        {
            vlcMediaPlayer.OnMediaPlayerBackward();
        }

        public override void Play()
        {
            vlcMediaPlayer.Play();
        }

        public override void Pause()
        {
            vlcMediaPlayer.OnMediaPlayerPaused();
        }

        public override void Stop()
        {
            vlcMediaPlayer.OnMediaPlayerStopped();
        }

        public override void Next(IMediaItem item)
        {
            vlcMediaPlayer.Stop();
            vlcMediaPlayer.SetMedia(new Uri(item.Location));
            vlcMediaPlayer.Play();
        }

        public override void Previous(IMediaItem item)
        {
            vlcMediaPlayer.Stop();
            vlcMediaPlayer.SetMedia(new Uri(item.Location));
            vlcMediaPlayer.Play();
        }

        public override void Play(IMediaItem item)
        {
            vlcMediaPlayer.Stop();
            vlcMediaPlayer.SetMedia(new Uri(item.Location));
            vlcMediaPlayer.Play();
        }
    }
}
