using Microsoft.Extensions.Logging;

namespace Maple.Log
{
    public static class Extensions
    {
        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory)
        {
            factory?.AddProvider(new Log4NetProvider());

            return factory;
        }
    }
}
