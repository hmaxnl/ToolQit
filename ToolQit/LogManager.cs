using System;
using ToolQit.Extensions;
using ToolQit.Logging;

namespace ToolQit
{
    public static class LogManager
    {
        public static ILog CreateLogger(string sender)
        {
            if (sender.IsNullEmptyWhiteSpace())
                throw new ArgumentException($"[{nameof(LogManager)}] sender cannot be null or empty!");
            return new Logger(sender, OnReceiveLog);
        }

        public static void RegisterAdapter(BaseLogAdapter adapter)
        {
            EmitLog += adapter.OnReceive;
            adapter.DisposeAdapter += DisposeAdapter;
        }
        
        private static event Action<LogEntry>? EmitLog;

        private static void DisposeAdapter(BaseLogAdapter adapter)
        {
            EmitLog -= adapter.OnReceive;
            adapter.DisposeAdapter -= DisposeAdapter;
        }

        private static void OnReceiveLog(LogEntry entry) => EmitLog?.Invoke(entry);


        // OLD =================================================
        /*public static ILogTransmitter CreateLogger2(string name)
        {
            return new LogTransmitter(name, ReceiveFromTransmitter);
        }

        public static void RegisterLogReceiver(ILogReceiver logReceiver)
        {
            if (logReceiver != null)
            {
                Transmit += logReceiver.Receive;
            }
        }
        private static event Action<LogEntry, ILogTransmitter>? Transmit;
        private static event Action<LogEntry, ILogReceiver>? Receive;

        private static void ReceiveFromTransmitter(LogEntry logEntry, ILogTransmitter sender)
        {
            if (sender == null)
                return;
            Transmit?.Invoke(logEntry, sender);
        }*/
    }
}