using Maple.Core;
using System;

namespace Maple
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Maple.Core.ObservableObject" />
    public class ExceptionDialogViewModel : ObservableObject
    {
        private Exception _exception;
        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Exception Exception
        {
            get { return _exception; }
            set { SetValue(ref _exception, value, OnChanged: () => OnPropertyChanged(nameof(ExceptionText))); }
        }

        /// <summary>
        /// Gets the exception text.
        /// </summary>
        /// <value>
        /// The exception text.
        /// </value>
        public string ExceptionText => Exception?.ToString();
    }
}
