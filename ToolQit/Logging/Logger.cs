using System;

namespace ToolQit.Logging
{
    public class Logger : ILog
    {
        internal Logger(string sender, Action<LogEntry>? receiveLog)
        {
            EmitLog = receiveLog;
            Sender = sender;
        }
        public string Sender { get; }
        public event Action<LogEntry>? EmitLog;
        
        public void Information(string template, params object?[]? parameters) =>
            EmitLog?.Invoke(new LogEntry() { Level = LogLevel.Information, Template = template, Parameters = parameters, Logger = this });

        public void Notify(string template, params object?[]? parameters) =>
            EmitLog?.Invoke(new LogEntry() { Level = LogLevel.Notify, Template = template, Parameters = parameters, Logger = this });

        public void Warning(string template, params object?[]? parameters) =>
            EmitLog?.Invoke(new LogEntry() { Level = LogLevel.Warning, Template = template, Parameters = parameters, Logger = this });

        public void Error(Exception exception, string template, params object?[]? parameters) =>
            EmitLog?.Invoke(new LogEntry() { Level = LogLevel.Error, Template = template, Parameters = parameters, Exception = exception, Logger = this });

        public void Fatal(Exception exception, string template, params object?[]? parameters) =>
            EmitLog?.Invoke(new LogEntry() { Level = LogLevel.Fatal, Template = template, Parameters = parameters, Exception = exception, Logger = this });

        public void Trace(string template, params object?[]? parameters) =>
            EmitLog?.Invoke(new LogEntry() { Level = LogLevel.Trace, Template = template, Parameters = parameters, Logger = this });

        public void Debug(string template, params object?[]? parameters) =>
            EmitLog?.Invoke(new LogEntry() { Level = LogLevel.Debug, Template = template, Parameters = parameters, Logger = this });

        public void Verbose(string template, params object?[]? parameters) =>
            EmitLog?.Invoke(new LogEntry() { Level = LogLevel.Verbose, Template = template, Parameters = parameters, Logger = this });

        public void Dispose()
        {
        }
    }
}