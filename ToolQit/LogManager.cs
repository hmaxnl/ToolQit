using System;
using ToolQit.Logging;

namespace ToolQit
{
    //TODO: Need to be worked out!
    public static class LogManager
    {
        public static ILogTransmitter CreateLogger(string name)
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
        private static event Action<LogData, ILogTransmitter>? Transmit;

        private static void ReceiveFromTransmitter(LogData logData, ILogTransmitter sender)
        {
            if (sender == null)
                return;
            Transmit?.Invoke(logData, sender);
        }
    }
}