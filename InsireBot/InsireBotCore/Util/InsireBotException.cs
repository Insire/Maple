using System;
using System.Runtime.Serialization;

namespace InsireBotCore
{
    /// <summary>
    /// this is just a wrapper class, so that i know which exceptions were thrown by the bot framework and which might be thrown by some framework i used
    /// </summary>
    public class InsireBotCoreException : Exception
    {
        public InsireBotCoreException()
        {
        }

        public InsireBotCoreException(string data) : base(data)
        {
        }

        public InsireBotCoreException(string data, Exception exception) : base(data, exception)
        {
        }

        public InsireBotCoreException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}
