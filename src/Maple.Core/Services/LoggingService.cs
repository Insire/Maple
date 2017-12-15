using System;
using log4net;
using Maple.Domain;

namespace Maple.Core
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Maple.Core.ILoggingService" />
    public class LoggingService : ILoggingService
    {
        private readonly ILog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingService"/> class.
        /// </summary>
        public LoggingService()
        {
            log4net.Config.XmlConfigurator.Configure();
            _log = LogManager.GetLogger(typeof(LoggingService));
        }

        public void Debug(object message)
        {
            var text = (string)message;

            System.Diagnostics.Debug.WriteLine(text);
            _log.Debug(text);
        }

        /// <summary>
        /// Informations the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Debug(object message, Exception exception)
        {
            var text = (string)message;

            System.Diagnostics.Debug.WriteLine(text);
            _log.Debug(message, exception);
        }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Error(object message)
        {
            _log.Error(message);
        }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
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
            var text = (string)message;

            _log.Info(text);
        }

        /// <summary>
        /// Informations the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Info(object message, Exception exception)
        {
            var text = (string)message;

            _log.Info(message, exception);
        }

        /// <summary>
        /// Warns the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Warn(object message)
        {
            _log.Warn(message);
        }

        /// <summary>
        /// Warns the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Warn(object message, Exception exception)
        {
            _log.Warn(message, exception);
        }
    }
}
