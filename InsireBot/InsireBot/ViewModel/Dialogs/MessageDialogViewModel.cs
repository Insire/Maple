using Maple.Core;

namespace Maple
{
    public class MessageDialogViewModel : ObservableObject
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetValue(ref _message, value); }
        }
    }
}
