using InsireBot.Core;
using System;
using System.Diagnostics;

namespace InsireBot.Tests
{
    public class MockLog : IBotLog
    {
        public void Error(object message)
        {
            Debug.WriteLine(message);
        }

        public void Error(object message, Exception exception)
        {
            Debug.WriteLine(message);
        }

        public void Fatal(object message)
        {
            Debug.WriteLine(message);
        }

        public void Fatal(object message, Exception exception)
        {
            Debug.WriteLine(message);
        }

        public void Info(object message)
        {
            Debug.WriteLine(message);
        }

        public void Info(object message, Exception exception)
        {
            Debug.WriteLine(message);
        }

        public void Warn(object message)
        {
            Debug.WriteLine(message);
        }

        public void Warn(object message, Exception exception)
        {
            Debug.WriteLine(message);
        }
    }
}
