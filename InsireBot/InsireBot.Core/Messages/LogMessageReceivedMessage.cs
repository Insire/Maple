namespace Maple.Core
{
    public class LogMessageReceivedMessage : GenericMapleMessage<string>
    {
        public LogMessageReceivedMessage(object sender, string content) : base(sender, content)
        {
        }
    }
}
