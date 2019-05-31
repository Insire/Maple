using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace Maple
{
    internal sealed class EventIdEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddOrUpdateProperty(new LogEventProperty("EventId", new ScalarValue(EventIdHash.Compute(logEvent.MessageTemplate.Text))));
        }
    }
}
