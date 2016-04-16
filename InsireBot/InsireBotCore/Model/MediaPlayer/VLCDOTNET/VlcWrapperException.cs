using System;
using System.Runtime.Serialization;

namespace InsireBotCore
{
    public class VlcWrapperException : Exception
    {
        public VlcWrapperException() : base()
        {

        }

        public VlcWrapperException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {

        }

        public VlcWrapperException(string message) : base(message)
        {

        }

        public VlcWrapperException(string message, Exception exception) : base(message, exception)
        {

        }

        public VlcWrapperException(Exception exception) : base("An error occured", exception)
        {

        }
    }
}
