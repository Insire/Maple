using Maple.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;

namespace Maple
{
    public class SplashScreenViewModel : ObservableObject, ISplashScreenViewModel
    {
        private readonly IMapleLog _log;
        private readonly Queue<string> _queue;
        private readonly System.Timers.Timer _timer;

        private string _version;
        public string Version
        {
            get { return _version; }
            private set { SetValue(ref _version, value); }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            private set { SetValue(ref _message, value); }
        }

        public bool IsDisposed { get; private set; }
        public bool IsDisposing { get; private set; }

        public bool IsLoaded { get; private set; }
        public bool IsLoading { get; private set; }

        public ICommand LoadCommand { get; private set; }
        public ICommand DisposeCommand { get; private set; }

        private SplashScreenViewModel()
        {
            _queue = new Queue<string>();
            _timer = new System.Timers.Timer(100);
            _timer.Elapsed += _timer_Elapsed;
        }

        public SplashScreenViewModel(IMapleLog log, IVersionService version) : this()
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));

            Version = version.Get();
            InitializeCommands();
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_queue.Count > 0)
            {
                Message = _queue.Dequeue();
                Debug.WriteLine(Message);
            }
        }

        private void InitializeCommands()
        {
            LoadCommand = new RelayCommand(Load, CanLoad);
            DisposeCommand = new RelayCommand(Dispose, CanDispose);
        }

        public void Load()
        {
            IsLoaded = false;
            IsLoading = true;
            _timer.Start();
            _log.LogMessageReceived += LogMessageReceived;

            IsLoading = false;
            IsLoaded = true;
        }

        private bool CanLoad()
        {
            return !IsDisposed
                && !IsDisposing
                && !IsLoaded
                && !IsLoading;
        }

        private void LogMessageReceived(object sender, LogMessageReceivedEventEventArgs e)
        {
            _queue.Enqueue(e.Message);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            if (disposing)
            {
                IsDisposing = false;
                IsDisposing = true;
                _timer.Stop();
                _timer.Elapsed -= _timer_Elapsed;
                _timer.Dispose();

                // Free any other managed objects here.
            }

            // Free any unmanaged objects here.
            IsDisposed = true;
            IsDisposing = false;
        }

        public bool CanDispose()
        {
            return !IsDisposed
                && !IsDisposing
                && IsLoaded
                && IsLoading;
        }
    }
}
