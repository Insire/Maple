using System;
using System.ComponentModel;
using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple
{
    /// <summary>
    /// Decorator for logging messages from windows.squirrel
    /// </summary>
    public class SquirrelLogger : Splat.ILogger, ILoggingService
    {
        private readonly ILoggingService _log;

        public SquirrelLogger(ILoggingService log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log), $"{nameof(log)} {Resources.IsRequired}");
        }

        public void Error(object message)
        {
            _log.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            _log.Error(message, exception);
        }

        public void Fatal(object message)
        {
            _log.Fatal(message);
        }

        public void Fatal(object message, Exception exception)
        {
            _log.Fatal(message, exception);
        }

        public void Info(object message)
        {
            _log.Info(message);
        }

        public void Info(object message, Exception exception)
        {
            _log.Info(message, exception);
        }

        public void Warn(object message)
        {
            _log.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            _log.Warn(message, exception);
        }

        public void Write([Localizable(false)] string message, LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Error:
                    Error(message);
                    break;
                case LogLevel.Fatal:
                    Fatal(message);
                    break;
                case LogLevel.Debug:
                case LogLevel.Info:
                    Info(message);
                    break;
                case LogLevel.Warn:
                    Warn(message);
                    break;
            }
        }
    }
}
