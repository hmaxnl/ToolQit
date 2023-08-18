using System;
using ToolQit.Extensions;

namespace ToolQit.Logging
{
    public class LogTransmitter : ILogTransmitter
    {
        internal LogTransmitter(string name, Action<LogData, ILogTransmitter> transmitLog)
        {
            if (name.IsNullEmptyWhiteSpace())
                throw new ArgumentException($"[{nameof(LogTransmitter)}] Invalid name, cannot construct log pipe!");
            Name = name;
            _transmitLog = transmitLog;
        }
        private readonly Action<LogData, ILogTransmitter> _transmitLog;

        public long LogsTransmitted { get; private set; }
        public string Name { get; }

        public void Information(string template, params object?[]? parameters)
        {
            _transmitLog.Invoke(new LogData() { Level = LogLevel.Information, Template = template, Parameters = parameters }, this);
            LogsTransmitted++;
        }

        public void Notify(string template, params object?[]? parameters)
        {
            _transmitLog.Invoke(new LogData() { Level = LogLevel.Notify, Template = template, Parameters = parameters }, this);
            LogsTransmitted++;
        }

        public void Warning(string template, params object?[]? parameters)
        {
            _transmitLog.Invoke(new LogData() { Level = LogLevel.Warning, Template = template, Parameters = parameters }, this);
            LogsTransmitted++;
        }

        public void Error(Exception exception, string template, params object?[]? parameters)
        {
            _transmitLog.Invoke(new LogData() { Level = LogLevel.Error, Template = template, Parameters = parameters, Exception = exception }, this);
            LogsTransmitted++;
        }

        public void Fatal(Exception exception, string template, params object?[]? parameters)
        {
            _transmitLog.Invoke(new LogData() { Level = LogLevel.Fatal, Template = template, Parameters = parameters, Exception = exception }, this);
            LogsTransmitted++;
        }

        public void Trace(string template, params object?[]? parameters)
        {
            _transmitLog.Invoke(new LogData() { Level = LogLevel.Trace, Template = template, Parameters = parameters }, this);
            LogsTransmitted++;
        }

        public void Debug(string template, params object?[]? parameters)
        {
            _transmitLog.Invoke(new LogData() { Level = LogLevel.Debug, Template = template, Parameters = parameters }, this);
            LogsTransmitted++;
        }

        public void Verbose(string template, params object?[]? parameters)
        {
            _transmitLog.Invoke(new LogData() { Level = LogLevel.Verbose, Template = template, Parameters = parameters }, this);
            LogsTransmitted++;
        }
    }
}