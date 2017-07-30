﻿using Maple.Localization.Properties;
using System;
using System.Windows.Input;

namespace Maple.Core
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.Windows.Input.ICommand" />
    public class RelayCommand : ICommand
    {
        private readonly Action _execute = null;
        private readonly Func<bool> _canExecute = null;

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="methodToExecute">The method to execute.</param>
        /// <exception cref="System.ArgumentNullException">methodToExecute</exception>
        public RelayCommand(Action methodToExecute)
        {
            _execute = methodToExecute ?? throw new ArgumentNullException(nameof(methodToExecute), $"{nameof(methodToExecute)} {Resources.IsRequired}");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="methodToExecute">The method to execute.</param>
        /// <param name="canExecuteEvaluator">The can execute evaluator.</param>
        /// <exception cref="System.ArgumentNullException">canExecuteEvaluator</exception>
        public RelayCommand(Action methodToExecute, Func<bool> canExecuteEvaluator)
            : this(methodToExecute)
        {
            _canExecute = canExecuteEvaluator ?? throw new ArgumentNullException(nameof(canExecuteEvaluator), $"{nameof(canExecuteEvaluator)} {Resources.IsRequired}");
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute.Invoke();
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            _execute();
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Windows.Input.ICommand" />
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute = null;
        private readonly Predicate<T> _canExecute = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        public RelayCommand(Action<T> execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute), $"{nameof(execute)} {Resources.IsRequired}");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        /// <exception cref="System.ArgumentNullException">execute</exception>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
            : this(execute)
        {
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute), $"{nameof(canExecute)} {Resources.IsRequired}");
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
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

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }
    }
}
