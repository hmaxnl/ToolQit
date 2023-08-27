using System;
using Serilog;

namespace ToolQit.Logging.Serilog
{
    public class SerilogAdapter : BaseLogAdapter
    {
        public SerilogAdapter(ILogger serilogLogger)
        {
            _serilog = serilogLogger;
        }

        private readonly ILogger _serilog;
        
        public override void OnReceive(LogEntry entry)
        {
            switch (entry.Level)
            {
                case LogLevel.Information:
                case LogLevel.Notify:
                    _serilog.ForContext("Sender", entry.Logger.SenderType).Information(entry.Template, entry.Parameters);
                    break;
                case LogLevel.Warning:
                    _serilog.ForContext("Sender", entry.Logger.SenderType).Warning(entry.Template, entry.Parameters);
                    break;
                case LogLevel.Error:
                    _serilog.ForContext("Sender", entry.Logger.SenderType).Error(entry.Exception, entry.Template, entry.Parameters);
                    break;
                case LogLevel.Fatal:
                    _serilog.ForContext("Sender", entry.Logger.SenderType).Fatal(entry.Exception, entry.Template, entry.Parameters);
                    break;
                case LogLevel.Trace:
                case LogLevel.Debug:
                    _serilog.ForContext("Sender", entry.Logger.SenderType).Debug(entry.Template, entry.Parameters);
                    break;
                case LogLevel.Verbose:
                    _serilog.ForContext("Sender", entry.Logger.SenderType).Verbose(entry.Template, entry.Parameters);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.OnReceive(entry);
        }
    }
}