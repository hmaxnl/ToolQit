using System;

namespace ToolQit.Logging
{
    /// <summary>
    /// This interface is used to pipe logging to a logging library or own logging implementation.
    /// This will by it self not write any data to disk or any other location!
    /// </summary>
    public interface ILogPipe
    {
        public void Information(string template, params object?[]? parameters);
        public void Notify(string template, params object?[]? parameters);
        public void Warning(string template, params object?[]? parameters);
        public void Error(Exception exception, string template, params object?[]? parameters);
        public void Fatal(Exception exception, string template, params object?[]? parameters);
        
        public void Trace(string template, params object?[]? parameters);
        public void Debug(string template, params object?[]? parameters);
    }
}