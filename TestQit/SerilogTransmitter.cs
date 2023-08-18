using System;
using Serilog;
using Serilog.Events;
using ToolQit.Logging;

namespace TestQit
{
    public class SerilogTransmitter : ILogReceiver
    {
        public long LogsReceived { get; private set; }
        public void Receive(LogData data, ILogTransmitter sender)
        {
            switch (data.Level)
            {
                case LogLevel.Information:
                case LogLevel.Notify:
                    Log.Write(LogEventLevel.Information, data.Template, data.Parameters);
                    break;
                case LogLevel.Warning:
                    Log.Write(LogEventLevel.Warning, data.Template, data.Parameters);
                    break;
                case LogLevel.Error:
                    Log.Error(data.Exception, data.Template, data.Parameters);
                    break;
                case LogLevel.Fatal:
                    Log.Fatal(data.Exception, data.Template, data.Parameters);
                    break;
                case LogLevel.Trace:
                case LogLevel.Debug:
                    Log.Write(LogEventLevel.Debug, data.Template, data.Parameters);
                    break;
                case LogLevel.Verbose:
                    Log.Write(LogEventLevel.Verbose, data.Template, data.Parameters);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            LogsReceived++;
        }
    }
}