using System;
using System.Collections.Generic;
using ToolQit.Logging;

namespace ToolQit
{
    public static class LogManager
    {
        public static ILogPipe CreateLogger(string name)
        {
            return new LogPipe(name, HandleLogFromPipe);
        }

        public static void RegisterLogOutput(ILogOutput logOutput)
        {
            if (logOutput != null)
            {
                EmitToOutput += logOutput.Receive;
            }
        }

        private static HashSet<ILogOutput> _outputs = new HashSet<ILogOutput>();
        private static event Action<LogData, ILogPipe>? EmitToOutput;

        private static void HandleLogFromPipe(LogData logData, ILogPipe sender)
        {
            if (sender == null)
                return;
            EmitToOutput?.Invoke(logData, sender);
        }
    }
}