using System;
using System.Collections.Generic;
using System.Windows.Input;
using Maple.Domain;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Abstractions;
using MvvmScarletToolkit.Commands;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    public class SplashScreenViewModel : ViewModelBase, IDisposable
    {
        private readonly IScarletMessenger _messenger;
        private readonly Queue<string> _queue;

        private bool _disposed;
        private System.Timers.Timer _timer;

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

        private ICommand _loadCommand;
        public ICommand LoadCommand
        {
            get { return _loadCommand; }
            private set { SetValue(ref _loadCommand, value); }
        }

        private ICommand _disposeCommand;
        public ICommand DisposeCommand
        {
            get { return _disposeCommand; }
            private set { SetValue(ref _disposeCommand, value); }
        }

        private SplashScreenViewModel(IScarletCommandBuilder commandBuilder)
            : base(commandBuilder)
        {
            _queue = new Queue<string>();
            _timer = new System.Timers.Timer(150);
            _timer.Elapsed += Timer_Elapsed;
        }

        private SplashScreenViewModel(IScarletMessenger messenger, IScarletCommandBuilder commandBuilder)
            : this(commandBuilder)
        {
            _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));
            _messenger.Subscribe<LogMessageReceivedMessage>(LogMessageReceived);
        }

        public SplashScreenViewModel(IScarletMessenger messenger, IVersionService version, IScarletCommandBuilder commandBuilder)
            : this(messenger, commandBuilder)
        {
            Version = version.Get();
            InitializeCommands();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_queue.Count == 0)
                return;

            var test = _queue.Dequeue();
            Message = test;
        }

        private void InitializeCommands()
        {
            LoadCommand = new RelayCommand(CommandManager, Load, CanLoad);
            DisposeCommand = new RelayCommand(CommandManager, Dispose, CanDispose);
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

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            _disposed = true;

            if (disposing)
            {
                if (_timer != null)
                {
                    _timer.Stop();
                    _timer.Elapsed -= Timer_Elapsed;
                    _timer.Dispose();
                    _timer = null;
                }
            }

            base.Dispose(disposing);
        }

        public bool CanDispose()
        {
            return !IsDisposed;
        }
    }
}
