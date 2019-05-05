using System;
using log4net;
using Microsoft.Extensions.Logging;

namespace Maple.Log
{
    public sealed class Log4NetLogger : ILogger
    {
        private readonly ILog _log;

        public Log4NetLogger(string loggerRepository, string name)
        {
            _log = LogManager.GetLogger(loggerRepository, name);
        }

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    return _log.IsFatalEnabled;

                case LogLevel.Debug:
                case LogLevel.Trace:
                    return _log.IsDebugEnabled;

                case LogLevel.Error:
                    return _log.IsErrorEnabled;

                case LogLevel.Information:
                    return _log.IsInfoEnabled;

                case LogLevel.Warning:
                    return _log.IsWarnEnabled;

                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel));
            }
        }

        /// <inheritdoc />
        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            if (formatter == null)
                throw new ArgumentNullException(nameof(formatter));

            var message = formatter(state, exception);
            if (!string.IsNullOrEmpty(message) || exception != null)
            {
                switch (logLevel)
                {
                    case LogLevel.Critical:
                        _log.Fatal(message, exception);
                        break;

                    case LogLevel.Debug:
                        _log.Debug(message, exception);
                        break;

                    case LogLevel.Error:
                        _log.Error(message, exception);
                        break;

                    case LogLevel.Information:
                        _log.Info(message, exception);
                        break;

                    case LogLevel.Warning:
                        _log.Warn(message, exception);
                        break;

                    case LogLevel.Trace:
                        _log.Debug(message, exception);
                        break;

                    default:
                        _log.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
                        _log.Info(message, exception);
                        break;
                }
            }
        }
    }
}
