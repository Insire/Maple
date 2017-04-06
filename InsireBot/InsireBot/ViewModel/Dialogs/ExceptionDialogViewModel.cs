using Maple.Core;
using System;

namespace Maple
{
    public class ExceptionDialogViewModel : ObservableObject
    {
        private Exception _exception;
        public Exception Exception
        {
            get { return _exception; }
            set { SetValue(ref _exception, value, OnChanged: () => OnPropertyChanged(nameof(ExceptionText))); }
        }

        public string ExceptionText => Exception?.ToString();
    }
}
