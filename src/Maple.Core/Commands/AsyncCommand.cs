using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maple.Core
{
    // source: https://msdn.microsoft.com/en-us/magazine/dn630647.aspx?f=255&MSPPError=-2147217396
    // Async Programming : Patterns for Asynchronous MVVM Applications: Commands by Stephen Cleary
    public class AsyncCommand<TResult> : AsyncCommandBase, INotifyPropertyChanged
    {
        private readonly Func<CancellationToken, Task<TResult>> _command;
        private readonly CancelAsyncCommand _cancelCommand;
        private readonly Func<bool> _canExecute = null;

        private NotifyTaskCompletion<TResult> _execution;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand CancelCommand => _cancelCommand;

        public NotifyTaskCompletion<TResult> Execution
        {
            get { return _execution; }
            private set
            {
                _execution = value;
                OnPropertyChanged();
            }
        }

        public AsyncCommand(Func<CancellationToken, Task<TResult>> command)
        {
            _command = command ?? throw new ArgumentNullException(nameof(command));
            _cancelCommand = new CancelAsyncCommand();
        }

        public AsyncCommand(Func<CancellationToken, Task<TResult>> command, Func<bool> canExecuteEvaluator)
            : this(command)
        {
            _canExecute = canExecuteEvaluator ?? throw new ArgumentNullException(nameof(canExecuteEvaluator));
        }

        public override bool CanExecute(object parameter)
        {
            return (Execution == null || Execution.IsCompleted)
                && (_canExecute?.Invoke() ?? true);
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _cancelCommand.NotifyCommandStarting();

            Execution = new NotifyTaskCompletion<TResult>(_command(_cancelCommand.Token));
            RaiseCanExecuteChanged();

            await Execution.TaskCompletion.ConfigureAwait(true);
            _cancelCommand.NotifyCommandFinished();
            RaiseCanExecuteChanged();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private sealed class CancelAsyncCommand : ICommand
        {
            private CancellationTokenSource _cts = new CancellationTokenSource();
            private bool _commandExecuting;

            public CancellationToken Token { get { return _cts.Token; } }

            public void NotifyCommandStarting()
            {
                _commandExecuting = true;
                if (!_cts.IsCancellationRequested)
                    return;

                _cts = new CancellationTokenSource();
                RaiseCanExecuteChanged();
            }

            public void NotifyCommandFinished()
            {
                _commandExecuting = false;
                RaiseCanExecuteChanged();
            }

            bool ICommand.CanExecute(object parameter)
            {
                return _commandExecuting && !_cts.IsCancellationRequested;
            }

            void ICommand.Execute(object parameter)
            {
                _cts.Cancel();
                RaiseCanExecuteChanged();
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            private void RaiseCanExecuteChanged()
            {
                CommandManager.InvalidateRequerySuggested();
            }
        }
    }

    public static class AsyncCommand
    {
        public static AsyncCommand<object> Create(Func<Task> command)
        {
            return new AsyncCommand<object>(async _ =>
            {
                await command().ConfigureAwait(true);
                return null;
            });
        }

        public static AsyncCommand<object> Create(Func<Task> command, Func<bool> canExecute)
        {
            return new AsyncCommand<object>(async _ =>
            {
                await command().ConfigureAwait(true);
                return null;
            }, canExecute);
        }

        public static AsyncCommand<TResult> Create<TResult>(Func<Task<TResult>> command, Func<bool> canExecute)
        {
            return new AsyncCommand<TResult>(_ => command(), canExecute);
        }

        public static AsyncCommand<object> Create(Func<CancellationToken, Task> command, Func<bool> canExecute)
        {
            return new AsyncCommand<object>(async token =>
            {
                await command(token).ConfigureAwait(true);
                return null;
            }, canExecute);
        }

        public static AsyncCommand<TResult> Create<TResult>(Func<CancellationToken, Task<TResult>> command, Func<bool> canExecute)
        {
            return new AsyncCommand<TResult>(command, canExecute);
        }
    }
}
