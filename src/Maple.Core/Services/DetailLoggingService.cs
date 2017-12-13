using System;
using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple.Core
{
    public class DetailLoggingService : ILoggingService
    {
        private readonly ILoggingService _log;

        private bool _hasLoggedException = false;

        public DetailLoggingService(ILoggingService log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log), $"{nameof(log)} {Resources.IsRequired}");
        }

        public void Error(object message)
        {
            if (!_hasLoggedException)
                _log.Error(DiagnosticReport.Generate(DiagnosticReportType.Full));

            _log.Error(message);

            _hasLoggedException = true;
        }

        public void Error(object message, Exception exception)
        {
            if (!_hasLoggedException)
                _log.Error(DiagnosticReport.Generate(DiagnosticReportType.Full));

            _log.Error(message, exception);

            _hasLoggedException = true;
        }

        public void Fatal(object message)
        {
            if (!_hasLoggedException)
                _log.Fatal(DiagnosticReport.Generate(DiagnosticReportType.Full));

            _log.Fatal(message);

            _hasLoggedException = true;
        }

        public void Fatal(object message, Exception exception)
        {
            if (!_hasLoggedException)
                _log.Fatal(DiagnosticReport.Generate(DiagnosticReportType.Full));

            _log.Fatal(message, exception);

            _hasLoggedException = true;
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
    }
}
