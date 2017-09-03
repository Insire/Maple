using Maple.Core;

namespace Maple
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Maple.Core.ObservableObject" />
    public class MessageDialogViewModel : ObservableObject
    {
        private string _message;
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message
        {
            get { return _message; }
            set { SetValue(ref _message, value); }
        }
    }
}
