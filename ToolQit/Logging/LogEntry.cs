using System;

namespace ToolQit.Logging
{
    public struct LogEntry
    {
        public LogLevel Level;
        public string Template;
        public object?[]? Parameters;
        public Exception? Exception;
        public ILog Logger;
    }
    public enum LogLevel
    {
        Information = 0,
        Notify = 1,
        Warning = 2,
        Error = 3,
        Fatal = 4,
        Trace = 5,
        Debug = 6,
        Verbose = 7
    }
}