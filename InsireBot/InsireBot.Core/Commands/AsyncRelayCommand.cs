using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InsireBot.Core
{
    public class AsyncRelayCommand<T> : ICommand
    {
        private readonly Func<bool> _canExecute = null;
        private readonly Func<T, Task> _execute = null;
        private Task _task;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public AsyncRelayCommand(Func<T, Task> methodToExecute)
        {
            if (methodToExecute == null)
                throw new ArgumentNullException(nameof(methodToExecute));

            _execute = methodToExecute;
        }

        public AsyncRelayCommand(Func<T, Task> methodToExecute, Func<bool> canExecuteEvaluator) : this(methodToExecute)
        {
            if (canExecuteEvaluator == null)
                throw new ArgumentNullException(nameof(canExecuteEvaluator));

            _canExecute = canExecuteEvaluator;
        }

        public bool CanExecute(object parameter)
        {
            return _task == null || _task.IsCompleted;
        }

        public async void Execute(object parameter)
        {
            _task = _execute((T)parameter);

            await _task;
        }
    }

    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<bool> _canExecute = null;
        private readonly Func<Task> _execute = null;
        private Task _task;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public AsyncRelayCommand(Func<Task> methodToExecute)
        {
            if (methodToExecute == null)
                throw new ArgumentNullException(nameof(methodToExecute));

            _execute = methodToExecute;
        }

        public AsyncRelayCommand(Func<Task> methodToExecute, Func<bool> canExecuteEvaluator) : this(methodToExecute)
        {
            if (canExecuteEvaluator == null)
                throw new ArgumentNullException(nameof(canExecuteEvaluator));

            _canExecute = canExecuteEvaluator;
        }

        public bool CanExecute(object parameter)
        {
            return _task == null || _task.IsCompleted;
        }

        public async void Execute(object parameter)
        {
            _task = _execute();

            await _task;
        }
    }
}
