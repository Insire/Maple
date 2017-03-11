using System;
using System.Windows.Input;

namespace Maple.Core
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute = null;
        private readonly Func<bool> _canExecute = null;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action methodToExecute)
        {
            _execute = methodToExecute ?? throw new ArgumentNullException(nameof(methodToExecute));
        }

        public RelayCommand(Action methodToExecute, Func<bool> canExecuteEvaluator) : this(methodToExecute)
        {
            _canExecute = canExecuteEvaluator ?? throw new ArgumentNullException(nameof(canExecuteEvaluator));
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute.Invoke();
        }

        public void Execute(object parameter)
        {
            _execute();
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute = null;
        private readonly Predicate<T> _canExecute = null;

        public RelayCommand(Action<T> execute) : this(execute, null)
        {
        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;

            if (parameter == null && typeof(T).IsValueType)
                return _canExecute.Invoke(default(T));

            if (parameter == null || parameter is T)
                return (_canExecute.Invoke((T)parameter));

            return false;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }
    }
}
