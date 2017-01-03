using System;
using System.Runtime.Serialization;

namespace InsireBot
{
    /// <summary>
    /// this is just a wrapper class, so that i know which exceptions were thrown by the bot and which might be thrown by some framework i used
    /// </summary>
    public class InsireBotException : Exception
    {
        public InsireBotException()
        {
        }

        public InsireBotException(string data) : base(data)
        {
        }

        public InsireBotException(string data, Exception exception) : base(data, exception)
        {
        }

        public InsireBotException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}
