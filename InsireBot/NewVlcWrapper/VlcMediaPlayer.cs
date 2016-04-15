using System;
using System.Runtime.InteropServices;
using GalaSoft.MvvmLight;
using Microsoft.Win32.SafeHandles;
using System.IO;

namespace NewVlcWrapper
{
    public class VlcMediaPlayer : ObservableObject, IDisposable
    {
        private SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        private Vlc.DotNet.Core.VlcMediaPlayer _vlcMediaPlayer;

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

            _vlcMediaPlayer.SetMedia(url);
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
