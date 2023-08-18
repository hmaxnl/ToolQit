using System;

namespace ToolQit.Logging
{
    public class LogOutput : ILogOutput
    {
        public long LogsHandled { get; private set; }
        public void Receive(LogData data, ILogPipe sender)
        {
            switch (data.Level)
            {
                case LogLevel.Information:
                case LogLevel.Notify:
                    Console.WriteLine($"[{sender.Name}] [Info - {DateTime.Now:hh:mm:ss}] {data.Template}");
                    break;
                case LogLevel.Warning:
                    Console.WriteLine($"[{sender.Name}] [Warning - {DateTime.Now:hh:mm:ss}] {data.Template}");
                    break;
                case LogLevel.Error:
                    Console.WriteLine($"[{sender.Name}] [Error - {DateTime.Now:hh:mm:ss}] {data.Template}");
                    if (data.Exception != null)
                        Console.WriteLine(data.Exception.Message);
                    break;
                case LogLevel.Fatal:
                    Console.WriteLine($"[{sender.Name}] [Fatal! - {DateTime.Now:hh:mm:ss}] {data.Template}");
                    if (data.Exception != null)
                        Console.WriteLine(data.Exception.Message);
                    break;
                case LogLevel.Trace:
                    Console.WriteLine($"[{sender.Name}] [Trace - {DateTime.Now:hh:mm:ss}] {data.Template}");
                    break;
                case LogLevel.Debug:
                    Console.WriteLine($"[{sender.Name}] [Debug - {DateTime.Now:hh:mm:ss}] {data.Template}");
                    break;
                case LogLevel.Verbose:
                    Console.WriteLine($"[{sender.Name}] [Verbose - {DateTime.Now:hh:mm:ss}] {data.Template}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            LogsHandled++;
        }
    }
}