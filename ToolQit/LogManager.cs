using System;
using System.Collections.Generic;
using ToolQit.Logging;


namespace ToolQit
{
    public static class LogManager
    {
        public static ILog CreateLogger(Type sender)
        {
            if (sender == null)
                throw new ArgumentException($"[{nameof(LogManager)}] sender cannot be null!");
            return new Logger(sender, OnReceiveLog);
        }

        public static bool RegisterAdapter(BaseLogAdapter adapter)
        {
            if (!AdapterSet.Add(adapter)) return false;
            adapter.RegisterEvent(ref EmitLog);
            return true;
        }

        public static void UnregisterAdapter(BaseLogAdapter adapter)
        {
            if (AdapterSet.Remove(adapter))
                adapter.Dispose();
        }

        internal static event Action<LogEntry>? EmitLog;
        private static readonly HashSet<BaseLogAdapter> AdapterSet = new HashSet<BaseLogAdapter>();
        private static void OnReceiveLog(LogEntry entry) => EmitLog?.Invoke(entry);
    }
}