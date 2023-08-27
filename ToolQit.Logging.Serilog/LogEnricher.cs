using Serilog.Core;
using Serilog.Events;

namespace ToolQit.Logging.Serilog
{
    public class LogEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            string sender = "Static";
            if (logEvent.Properties.TryGetValue("SourceContext", out LogEventPropertyValue propVal))
                sender = propVal.ToString();
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Sender", sender));
        }
    }
}