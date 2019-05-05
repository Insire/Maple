using System.Collections.Concurrent;
using System.Reflection;
using log4net;
using log4net.Repository;
using Microsoft.Extensions.Logging;

namespace Maple.Log
{
    public sealed class Log4NetProvider : ILoggerProvider
    {
        private readonly ILoggerRepository _loggerRepository;
        private readonly ConcurrentDictionary<string, Log4NetLogger> _loggers = new ConcurrentDictionary<string, Log4NetLogger>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="Log4NetProvider" /> class.
        /// </summary>
        public Log4NetProvider()
        {
            _loggerRepository = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(Log4NetProvider));
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }

        /// <inheritdoc />
        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, _ => new Log4NetLogger(_loggerRepository.Name, categoryName));
        }
    }
}
