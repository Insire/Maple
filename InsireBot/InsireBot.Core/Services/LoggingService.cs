using log4net;
using System;
using System.Diagnostics;

namespace Maple.Core
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Maple.Core.IMapleLog" />
    public class LoggingService : IMapleLog
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

        public event LogMessageReceivedEventHandler LogMessageReceived;

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

        /// <summary>
        /// Fatals the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Fatal(object message)
        {
            _log.Fatal(message);
        }

        /// <summary>
        /// Fatals the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Fatal(object message, Exception exception)
        {
            _log.Fatal(message, exception);
        }

        /// <summary>
        /// Informations the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info(object message)
        {
            var text = (string)message;

            Debug.WriteLine(text);
            _log.Info(text);

            LogMessageReceived?.Invoke(this, new LogMessageReceivedEventEventArgs(text));
        }

        /// <summary>
        /// Informations the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Info(object message, Exception exception)
        {
            var text = (string)message;

            Debug.WriteLine(text);
            _log.Info(message, exception);

            LogMessageReceived?.Invoke(this, new LogMessageReceivedEventEventArgs(text));
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
