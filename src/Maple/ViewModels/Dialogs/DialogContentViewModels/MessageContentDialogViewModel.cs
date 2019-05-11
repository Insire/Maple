using MvvmScarletToolkit.Observables;

namespace Maple
{
    public class MessageContentDialogViewModel : ObservableObject
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetValue(ref _message, value); }
        }
    }
}
