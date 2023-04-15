using System;
using System.IO;
using Serilog;
using Serilog.Formatting.Json;
using ToolQit;

namespace TestQit
{
    internal static class Program
    {
        public static void Main()
        {
            Log.Logger = LogConfig.CreateLogger();
            Console.WriteLine("TestQit!");
            var testVal = Caretaker.Settings.GetString("Test");
            Console.WriteLine("Value from settings: {0}", testVal);
        }
        
        private static LoggerConfiguration LogConfig { get; set; } = new LoggerConfiguration()
        
#if DEBUG
            .WriteTo.Debug()
            .MinimumLevel.Verbose()
#endif
            .WriteTo.Console()
            .WriteTo.File(formatter:new JsonFormatter(), path:Path.Combine(Environment.CurrentDirectory, "Logs", "log_.json"), rollingInterval: RollingInterval.Day);
    }
}