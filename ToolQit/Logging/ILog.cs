using System;

namespace ToolQit.Logging
{
    public interface ILog : IDisposable
    {
        public string Sender { get; }
        public event Action<LogEntry> EmitLog;
        
        public void Information(string template, params object?[]? parameters);
        public void Notify(string template, params object?[]? parameters);
        public void Warning(string template, params object?[]? parameters);
        public void Error(Exception exception, string template, params object?[]? parameters);
        public void Fatal(Exception exception, string template, params object?[]? parameters);
        
        public void Trace(string template, params object?[]? parameters);
        public void Debug(string template, params object?[]? parameters);
        public void Verbose(string template, params object?[]? parameters);
    }
}