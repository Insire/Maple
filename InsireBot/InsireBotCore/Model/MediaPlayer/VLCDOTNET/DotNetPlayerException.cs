using System;
using System.Runtime.Serialization;

namespace InsireBotCore
{
    [Serializable()]
    public class DotNetPlayerException : Exception
    {
        public DotNetPlayerException() : base()
        {
        }

        public DotNetPlayerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public DotNetPlayerException(string message) : base(message)
        {
        }

        public DotNetPlayerException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
