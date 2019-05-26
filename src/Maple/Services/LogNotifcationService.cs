using System;
using Maple.Core;
using Maple.Domain;
using Maple.Localization.Properties;
using MvvmScarletToolkit.Abstractions;

namespace Maple
{
    public sealed class LoggingNotifcationService : ILoggingNotifcationService
    {
        private readonly ILoggingService _log;
        private readonly IMessenger _messenger;

        public LoggingNotifcationService(IMessenger messenger, ILoggingService log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log), $"{nameof(log)} {Resources.IsRequired}");
            _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger), $"{nameof(messenger)} {Resources.IsRequired}");
        }

        public void Info(object message)
        {
            _log.Info(message);

            if (message is string text)
                _messenger.Publish(new LogMessageReceivedMessage(this, text));
        }

        public void Warn(object message)
        {
            _log.Warn(message);
        }

        public void Error(object message, Exception exception)
        {
            _log.Error(message, exception);
        }

        public void Fatal(object message, Exception exception)
        {
            _log.Fatal(message, exception);
        }
    }
}
