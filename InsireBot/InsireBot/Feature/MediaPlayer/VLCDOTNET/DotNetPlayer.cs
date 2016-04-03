using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Vlc.DotNet.Core;

namespace InsireBot.MediaPlayer
{
    public sealed class DotNetPlayer : BasePlayer, IMediaPlayer<IMediaItem>
    {
        private VlcMediaPlayer vlcMediaPlayer;

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
            vlcMediaPlayer = new VlcMediaPlayer(Settings.VlcLibDirectory,Settings.Options);

            vlcMediaPlayer.Buffering += VlcMediaPlayer_Buffering;
            vlcMediaPlayer.EncounteredError += VlcMediaPlayer_EncounteredError;
            vlcMediaPlayer.EndReached += VlcMediaPlayer_EndReached;
            vlcMediaPlayer.MediaChanged += VlcMediaPlayer_MediaChanged;
            vlcMediaPlayer.Opening += VlcMediaPlayer_Opening;
            vlcMediaPlayer.Stopped += VlcMediaPlayer_Stopped;
            vlcMediaPlayer.TitleChanged += VlcMediaPlayer_TitleChanged;
            vlcMediaPlayer.VideoOutChanged += VlcMediaPlayer_VideoOutChanged;
        }

        private void VlcMediaPlayer_VideoOutChanged(object sender, VlcMediaPlayerVideoOutChangedEventArgs e)
        {
            Debug.WriteLine("VlcMediaPlayer_VideoOutChanged");
        }

        private void VlcMediaPlayer_TitleChanged(object sender, VlcMediaPlayerTitleChangedEventArgs e)
        {
            Debug.WriteLine("VlcMediaPlayer_TitleChanged");
        }

        private void VlcMediaPlayer_Stopped(object sender, VlcMediaPlayerStoppedEventArgs e)
        {
            Debug.WriteLine("VlcMediaPlayer_Stopped");
        }

        private void VlcMediaPlayer_Opening(object sender, VlcMediaPlayerOpeningEventArgs e)
        {
            Debug.WriteLine("VlcMediaPlayer_Opening");
        }

        private void VlcMediaPlayer_MediaChanged(object sender, VlcMediaPlayerMediaChangedEventArgs e)
        {
            Debug.WriteLine("VlcMediaPlayer_MediaChanged");
        }

        private void VlcMediaPlayer_EndReached(object sender, VlcMediaPlayerEndReachedEventArgs e)
        {
            Debug.WriteLine("VlcMediaPlayer_EndReached");
        }

        private void VlcMediaPlayer_EncounteredError(object sender, VlcMediaPlayerEncounteredErrorEventArgs e)
        {
            Debug.WriteLine("VlcMediaPlayer_EncounteredError");
        }

        private void VlcMediaPlayer_Buffering(object sender, VlcMediaPlayerBufferingEventArgs e)
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
            vlcMediaPlayer.OnMediaPlayerPaused();
        }

        public override void Stop()
        {
            vlcMediaPlayer.OnMediaPlayerStopped();
        }

        public override void Play(IMediaItem item)
        {
            vlcMediaPlayer.Stop();

            if (item != null)
            {
                vlcMediaPlayer.SetMedia(new Uri(item.Location), new string[0]);
                vlcMediaPlayer.Play();
            }
        }
    }
}
