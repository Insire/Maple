using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using GalaSoft.MvvmLight;
using Microsoft.Win32.SafeHandles;
using Vlc.DotNet.Core;
using Vlc.DotNet.Core.Medias;
using Vlc.DotNet.Wpf;
using static Vlc.DotNet.Core.VlcLogProperties;

namespace VlcWrapper
{
    public class VlcMediaPlayer : ObservableObject, IDisposable
    {
        private SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        private VlcControl _vlcControl;

        public event VlcEventHandler<VlcControl, float> Buffering
        {
            add { _vlcControl.Buffering += value; }
            remove { _vlcControl.Buffering -= value; }
        }

        public event VlcEventHandler<VlcControl, EventArgs> EncounteredError
        {
            add { _vlcControl.EncounteredError += value; }
            remove { _vlcControl.EncounteredError -= value; }
        }

        public event VlcEventHandler<VlcControl, EventArgs> EndReached
        {
            add { _vlcControl.EndReached += value; }
            remove { _vlcControl.EndReached -= value; }
        }

        public event EventHandler Initialized
        {
            add { _vlcControl.Initialized += value; }
            remove { _vlcControl.Initialized -= value; }
        }

        public event VlcEventHandler<VlcControl, int> PausableChanged
        {
            add { _vlcControl.PausableChanged += value; }
            remove { _vlcControl.PausableChanged -= value; }
        }

        public event VlcEventHandler<VlcControl, EventArgs> Paused
        {
            add { _vlcControl.Paused += value; }
            remove { _vlcControl.Paused -= value; }
        }

        public event VlcEventHandler<VlcControl, EventArgs> Playing
        {
            add { _vlcControl.Playing += value; }
            remove { _vlcControl.Playing -= value; }
        }

        public event VlcEventHandler<VlcControl, float> PositionChanged
        {
            add { _vlcControl.PositionChanged += value; }
            remove { _vlcControl.PositionChanged -= value; }
        }

        public event VlcEventHandler<VlcControl, EventArgs> Stopped
        {
            add { _vlcControl.Stopped += value; }
            remove { _vlcControl.Stopped -= value; }
        }

        public event VlcEventHandler<VlcControl, TimeSpan> TimeChanged
        {
            add { _vlcControl.TimeChanged += value; }
            remove { _vlcControl.TimeChanged -= value; }
        }

        public event VlcEventHandler<VlcControl, long> TitleChanged
        {
            add { _vlcControl.TitleChanged += value; }
            remove { _vlcControl.TitleChanged -= value; }
        }

        public event RoutedEventHandler Unloaded
        {
            add { _vlcControl.Unloaded += value; }
            remove { _vlcControl.Unloaded -= value; }
        }

        public int Volume
        {
            get { return _vlcControl.AudioProperties.Volume; }
            set
            {
                _vlcControl.AudioProperties.Volume = value;
                RaisePropertyChanged(nameof(Volume));
            }
        }

        public bool IsMute
        {
            get { return _vlcControl.AudioProperties.IsMute; }
            set
            {
                _vlcControl.AudioProperties.IsMute = value;
                RaisePropertyChanged(nameof(IsMute));
            }
        }

        public bool IsPlaying
        {
            get { return _vlcControl.IsPlaying; }
        }

        public VlcLogMessages LogMessages
        {
            get { return _vlcControl.LogProperties.LogMessages; }
        }

        public float Position
        {
            get { return _vlcControl.Position; }
            set
            {
                _vlcControl.Position = value;
                RaisePropertyChanged(nameof(Position));
            }
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

        public VlcMediaPlayer(DirectoryInfo directoryInfo, string[] arguments)
        {
            ValidateParameters(directoryInfo, arguments);
            InitializeOptions(directoryInfo, arguments);
            InitializePlayer();
        }

        public VlcMediaPlayer(DirectoryInfo directoryInfo, VlcMediaPlayerOptions vlcMediaPlayerOptions, string[] arguments)
        {
            ValidateParameters(directoryInfo, arguments);
            InitializeOptions(directoryInfo, arguments);
            InitializePlayer(vlcMediaPlayerOptions);
        }

        private void InitializeOptions(DirectoryInfo directoryInfo, string[] arguments)
        {
            var path = directoryInfo.FullName;
            VlcContext.LibVlcDllsPath = path;
            VlcContext.LibVlcPluginsPath = GetPluginsFolderPath(directoryInfo).FullName;

            foreach (var option in arguments)
                VlcContext.StartupOptions.AddOption(option);
        }

        private void InitializePlayer(VlcMediaPlayerOptions vlcMediaPlayerOptions)
        {
            _vlcControl = new VlcControl();

            Volume = vlcMediaPlayerOptions.Volume;
            IsMute = vlcMediaPlayerOptions.IsMuted;
        }

        private void InitializePlayer()
        {
            var options = new VlcMediaPlayerOptions();
            options.IsMuted = false;
            options.Volume = 100;

            InitializePlayer(options);
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

            if (_vlcControl.IsPlaying)
                Stop();

            if (disposing)
            {
                handle.Dispose();

                foreach (var media in _vlcControl.Medias)
                    media.Dispose();

                _vlcControl.Media?.Dispose();

                _vlcControl.Medias.Clear();
                _vlcControl.Dispose();
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            Disposed = true;
        }

        public void Play(Uri url)
        {
            if (_vlcControl.IsPlaying || _vlcControl.IsPaused)
                _vlcControl.Stop();

            var disposeableItems = _vlcControl.Medias;
            foreach (var item in _vlcControl.Medias)
                _vlcControl.Medias.Remove(item);

            var count = disposeableItems.Count;
            if (count > 0)
                for (var i = count; i > -1; --i)
                {
                    disposeableItems[i].Dispose();
                }

            if (url.IsFile)
                PlayLocal(url);
            else
                PlayWeb(url);
        }

        private void PlayLocal(Uri url)
        {
            MediaBase mediaBase = new PathMedia(url.OriginalString);
            Play(mediaBase);
        }

        private void PlayWeb(Uri url)
        {
            MediaBase mediaBase = new LocationMedia(url.OriginalString);
            Play(mediaBase);
        }

        private void Play(MediaBase mediaBase)
        {
            _vlcControl.Play(mediaBase);
            RaisePropertyChanged(nameof(IsPlaying));
        }

        public void Stop()
        {
            _vlcControl.Stop();
            RaisePropertyChanged(nameof(IsPlaying));
        }

        public void Pause()
        {
            _vlcControl.Pause();
            RaisePropertyChanged(nameof(IsPlaying));
        }
    }
}
