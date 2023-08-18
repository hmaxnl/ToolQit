using System;
using ToolQit.Extensions;

namespace ToolQit.Logging
{
    public class LogPipe : ILogPipe
    {
        internal LogPipe(string name, Action<LogData, ILogPipe> emitLog)
        {
            if (name.IsNullEmptyWhiteSpace())
                throw new ArgumentException($"[{nameof(LogPipe)}] Invalid name, cannot construct log pipe!");
            Name = name;
            _emitLog = emitLog;
        }
        private readonly Action<LogData, ILogPipe> _emitLog;

        public long LogCount { get; private set; }
        public string Name { get; }

        public void Information(string template, params object?[]? parameters)
        {
            _emitLog.Invoke(new LogData() { Level = LogLevel.Information, Template = template, Parameters = parameters }, this);
            LogCount++;
        }

        public void Notify(string template, params object?[]? parameters)
        {
            _emitLog.Invoke(new LogData() { Level = LogLevel.Notify, Template = template, Parameters = parameters }, this);
            LogCount++;
        }

        public void Warning(string template, params object?[]? parameters)
        {
            _emitLog.Invoke(new LogData() { Level = LogLevel.Warning, Template = template, Parameters = parameters }, this);
            LogCount++;
        }

        public void Error(Exception exception, string template, params object?[]? parameters)
        {
            _emitLog.Invoke(new LogData() { Level = LogLevel.Error, Template = template, Parameters = parameters, Exception = exception }, this);
            LogCount++;
        }

        public void Fatal(Exception exception, string template, params object?[]? parameters)
        {
            _emitLog.Invoke(new LogData() { Level = LogLevel.Fatal, Template = template, Parameters = parameters, Exception = exception }, this);
            LogCount++;
        }

        public void Trace(string template, params object?[]? parameters)
        {
            _emitLog.Invoke(new LogData() { Level = LogLevel.Trace, Template = template, Parameters = parameters }, this);
            LogCount++;
        }

        public void Debug(string template, params object?[]? parameters)
        {
            _emitLog.Invoke(new LogData() { Level = LogLevel.Debug, Template = template, Parameters = parameters }, this);
            LogCount++;
        }

        public void Verbose(string template, params object?[]? parameters)
        {
            _emitLog.Invoke(new LogData() { Level = LogLevel.Verbose, Template = template, Parameters = parameters }, this);
            LogCount++;
        }
    }
}