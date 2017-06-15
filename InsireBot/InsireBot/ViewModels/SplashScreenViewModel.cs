using Maple.Core;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Maple
{
    public class SplashScreenViewModel : ObservableObject, ISplashScreenViewModel
    {
        private readonly IMessenger _messenger;
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

        public ICommand LoadCommand { get; private set; }
        public ICommand DisposeCommand { get; private set; }

        private SplashScreenViewModel()
        {
            _queue = new Queue<string>();
            _timer = new System.Timers.Timer(150);
            _timer.Elapsed += _timer_Elapsed;
        }

        private SplashScreenViewModel(IMessenger messenger) : this()
        {
            _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));
            _messenger.Subscribe<LogMessageReceivedMessage>(LogMessageReceived);
        }

        public SplashScreenViewModel(IMessenger messenger, IVersionService version) : this(messenger)
        {
            Version = version.Get();
            InitializeCommands();
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_queue.Count == 0)
                return;

            var test = _queue.Dequeue();
            Message = test;
        }

        private void InitializeCommands()
        {
            LoadCommand = new RelayCommand(Load, CanLoad);
            DisposeCommand = new RelayCommand(Dispose, CanDispose);
        }

        public void Load()
        {
            _timer.Start();
        }

        private bool CanLoad()
        {
            return !IsDisposed;
        }

        private void LogMessageReceived(LogMessageReceivedMessage e)
        {
            _queue.Enqueue(e.Content);
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
                _timer.Stop();
                _timer.Elapsed -= _timer_Elapsed;
                _timer.Dispose();

                // Free any other managed objects here.
            }

            // Free any unmanaged objects here.
            IsDisposed = true;
        }

        public bool CanDispose()
        {
            return !IsDisposed;
        }
    }
}
