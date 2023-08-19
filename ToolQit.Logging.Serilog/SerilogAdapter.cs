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
                    _serilog.Information(entry.Template, entry.Parameters);
                    break;
                case LogLevel.Warning:
                    _serilog.Warning(entry.Template, entry.Parameters);
                    break;
                case LogLevel.Error:
                    _serilog.Error(entry.Exception, entry.Template, entry.Parameters);
                    break;
                case LogLevel.Fatal:
                    _serilog.Fatal(entry.Exception, entry.Template, entry.Parameters);
                    break;
                case LogLevel.Trace:
                case LogLevel.Debug:
                    _serilog.Debug(entry.Template, entry.Parameters);
                    break;
                case LogLevel.Verbose:
                    _serilog.Verbose(entry.Template, entry.Parameters);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.OnReceive(entry);
        }
    }
}