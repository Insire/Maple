using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using GalaSoft.MvvmLight;
using Microsoft.Win32.SafeHandles;
using Vlc.DotNet.Core;

namespace InsireBotCore
{
    public class VlcMediaPlayer : ObservableObject, IDisposable
    {
        private SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        private Vlc.DotNet.Core.VlcMediaPlayer _vlcMediaPlayer;

        public event EventHandler<VlcMediaPlayerBufferingEventArgs> Buffering
        {
            add { _vlcMediaPlayer.Buffering += value; }
            remove { _vlcMediaPlayer.Buffering -= value; }
        }

        public event EventHandler<VlcMediaPlayerEncounteredErrorEventArgs> EncounteredError
        {
            add { _vlcMediaPlayer.EncounteredError += value; }
            remove { _vlcMediaPlayer.EncounteredError -= value; }
        }

        public event EventHandler<VlcMediaPlayerEndReachedEventArgs> EndReached
        {
            add { _vlcMediaPlayer.EndReached += value; }
            remove { _vlcMediaPlayer.EndReached -= value; }
        }

        public event EventHandler<VlcMediaPlayerPlayingEventArgs> Playing
        {
            add { _vlcMediaPlayer.Playing += value; }
            remove { _vlcMediaPlayer.Playing -= value; }
        }

        public event EventHandler<VlcMediaPlayerStoppedEventArgs> Stopped
        {
            add { _vlcMediaPlayer.Stopped += value; }
            remove { _vlcMediaPlayer.Stopped -= value; }
        }

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

        public int Volume
        {
            get { return _vlcMediaPlayer.Audio.Volume; }
            set
            {
                _vlcMediaPlayer.Audio.Volume = value;
                RaisePropertyChanged(nameof(Volume));
            }
        }

        public bool IsMute
        {
            get { return _vlcMediaPlayer.Audio.IsMute; }
            set
            {
                _vlcMediaPlayer.Audio.IsMute = value;
                RaisePropertyChanged(nameof(IsMute));
            }
        }

        public bool IsPlaying
        {
            get { return _vlcMediaPlayer.State == Vlc.DotNet.Core.Interops.Signatures.MediaStates.Playing; }
        }

        public bool IsPaused
        {
            get { return _vlcMediaPlayer.State == Vlc.DotNet.Core.Interops.Signatures.MediaStates.Paused; }
        }

        public VlcMediaPlayer(DirectoryInfo directoryInfo, string[] arguments)
        {
            ValidateParameters(directoryInfo, arguments);
            InitializePlayer(directoryInfo, arguments);
        }

        public VlcMediaPlayer(DirectoryInfo directoryInfo, MediaPlayerOptions vlcMediaPlayerOptions, string[] arguments)
        {
            ValidateParameters(directoryInfo, arguments);
            InitializePlayer(directoryInfo, vlcMediaPlayerOptions, arguments);
        }

        private void InitializePlayer(DirectoryInfo directoryInfo, string[] arguments)
        {
            var options = new MediaPlayerOptions
            {
                IsMuted = false,
                Volume = 100,
            };

            InitializePlayer(directoryInfo, options, arguments);
        }

        private void InitializePlayer(DirectoryInfo directoryInfo, MediaPlayerOptions vlcMediaPlayerOptions, string[] arguments)
        {
            _vlcMediaPlayer = new Vlc.DotNet.Core.VlcMediaPlayer(directoryInfo, arguments);

            Volume = vlcMediaPlayerOptions.Volume;
            IsMute = vlcMediaPlayerOptions.IsMuted;
        }

        private void ValidateParameters(DirectoryInfo directoryInfo, string[] arguments)
        {
            if (directoryInfo == null)
                throw new VlcWrapperException("Path to the local installation of Vlc can't be empty", new ArgumentNullException(nameof(directoryInfo)));

            if (!directoryInfo.Exists)
                throw new VlcWrapperException("Path to the local installation of Vlc has to exist", new ArgumentException(nameof(directoryInfo)));

            var folder = GetPluginsFolderPath(directoryInfo);
            if (!folder.Exists)
                throw new VlcWrapperException("Can't find plugins folder inside Vlc installation", new ArgumentException(nameof(directoryInfo)));

            if (arguments.Any(p => string.IsNullOrEmpty(p)))
                throw new VlcWrapperException("Commandline arguments can't contain empty values", new ArgumentException(nameof(arguments)));
        }

        private DirectoryInfo GetPluginsFolderPath(DirectoryInfo directoryInfo)
        {
            var path = directoryInfo.FullName;

            return new DirectoryInfo(Path.Combine(path, "plugins"));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (IsPlaying)
                Stop();

            if (disposing)
            {
                handle.Dispose();

                _vlcMediaPlayer.Dispose();
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            Disposed = true;
        }

        public void Play(FileInfo info)
        {
            if (IsPlaying)
                _vlcMediaPlayer.Stop();

            _vlcMediaPlayer.SetMedia(info);
            Play();
        }

        public void Play(string mrl)
        {
            if (IsPlaying)
                _vlcMediaPlayer.Stop();

            _vlcMediaPlayer.SetMedia(mrl);
            Play();
        }

        public void Play(Uri url)
        {
            if (IsPlaying)
                _vlcMediaPlayer.Stop();

            if (url.IsFile)
                _vlcMediaPlayer.SetMedia(url);
            else
            {
                var mrl = MrlExtractionService.GetMrls(url.OriginalString).First();
                _vlcMediaPlayer.SetMedia(mrl);
            }

            Play();
        }

        private void Play()
        {
            _vlcMediaPlayer.Play();
            RaisePropertyChanged(nameof(IsPlaying));
        }

        public void Stop()
        {
            _vlcMediaPlayer.Stop();
            RaisePropertyChanged(nameof(IsPlaying));
        }

        public void Pause()
        {
            _vlcMediaPlayer.Pause();
            RaisePropertyChanged(nameof(IsPlaying));
        }
    }
}
