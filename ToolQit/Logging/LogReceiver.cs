using System;

namespace ToolQit.Logging
{
    //TODO: Implement named formatting.
    public class LogReceiver : ILogReceiver
    {
        public long LogsReceived { get; private set; }
        public void Receive(LogData data, ILogTransmitter sender)
        {
            string formatted = data.Template;
            /*if (data.Parameters != null)
                formatted = string.Format(formatted, data.Parameters);*/
            switch (data.Level)
            {
                case LogLevel.Information:
                case LogLevel.Notify:
                    Console.WriteLine($"[{DateTime.Now:hh:mm:ss}] [{sender.Name} | Info -] {formatted}");
                    break;
                case LogLevel.Warning:
                    Console.WriteLine($"[{DateTime.Now:hh:mm:ss}] [{sender.Name} | Warning -] {formatted}");
                    break;
                case LogLevel.Error:
                    Console.WriteLine($"[{DateTime.Now:hh:mm:ss}] [{sender.Name} | Error -] {formatted}");
                    if (data.Exception != null)
                        Console.WriteLine(data.Exception.Message);
                    break;
                case LogLevel.Fatal:
                    Console.WriteLine($"[{DateTime.Now:hh:mm:ss}] [{sender.Name} | Fatal -] {formatted}");
                    if (data.Exception != null)
                        Console.WriteLine(data.Exception.Message);
                    break;
                case LogLevel.Trace:
                    Console.WriteLine($"[{DateTime.Now:hh:mm:ss}] [{sender.Name} | Trace -] {formatted}");
                    break;
                case LogLevel.Debug:
                    Console.WriteLine($"[{DateTime.Now:hh:mm:ss}] [{sender.Name} | Debug -] {formatted}");
                    break;
                case LogLevel.Verbose:
                    Console.WriteLine($"[{DateTime.Now:hh:mm:ss}] [{sender.Name} | Verbose -] {formatted}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            LogsReceived++;
        }
    }
}