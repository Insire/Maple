using MvvmScarletToolkit;

namespace Maple
{
    public class LogMessageReceivedMessage : GenericScarletMessage<string>
    {
        public LogMessageReceivedMessage(object sender, string content)
            : base(sender, content)
        {
        }
    }
}
