using System;
using System.ComponentModel;
using Microsoft.Extensions.Logging;
using LogLevel = Splat.LogLevel;

namespace Maple
{
    /// <summary>
    /// Decorator for logging messages from windows.squirrel
    /// </summary>
    public sealed class SquirrelLogger : Splat.ILogger
    {
        private readonly ILogger _log;

        public LogLevel Level { get; set; }

        public SquirrelLogger(ILoggerFactory factory)
        {
            _log = factory.CreateLogger<SquirrelLogger>() ?? throw new ArgumentNullException(nameof(factory));
        }

        public void Error(string message)
        {
            _log.LogError(message);
        }

        public void Error(string message, Exception exception)
        {
            _log.LogError(message, exception);
        }

        public void Fatal(string message)
        {
            _log.LogCritical(message);
        }

        public void Fatal(string message, Exception exception)
        {
            _log.LogCritical(message, exception);
        }

        public void Info(string message)
        {
            _log.LogInformation(message);
        }

        public void Info(string message, Exception exception)
        {
            _log.LogInformation(message, exception);
        }

        public void Warn(string message)
        {
            _log.LogWarning(message);
        }

        public void Warn(string message, Exception exception)
        {
            _log.LogWarning(message, exception);
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
