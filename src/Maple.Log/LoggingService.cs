using System;
using Maple.Domain;
using NLog;

namespace Maple.Log
{
    public sealed class LoggingService : ILoggingService
    {
        private readonly ILogger _log;

        public LoggingService()
        {
            _log = LogManager.GetCurrentClassLogger();
        }

        public void Debug(object message)
        {
            var text = (string)message;

            System.Diagnostics.Debug.WriteLine(text);
            _log.Debug(text);
        }

        public void Debug(object message, Exception exception)
        {
            var text = (string)message;

            System.Diagnostics.Debug.WriteLine(text);
            _log.Debug(exception, text);
        }

        public void Error(object message)
        {
            _log.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            _log.Error(exception, (string)message);
        }

        public void Fatal(object message)
        {
            _log.Fatal(message);
        }

        public void Fatal(object message, Exception exception)
        {
            _log.Fatal(exception, (string)message);
        }

        public void Info(object message)
        {
            var text = (string)message;

            _log.Info(text);
        }

        public void Info(object message, Exception exception)
        {
            _log.Info(exception, (string)message);
        }

        public void Warn(object message)
        {
            _log.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            _log.Warn(exception, (string)message);
        }
    }
}
