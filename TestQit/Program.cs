using System;
using Serilog;
using ToolQit;
using ToolQit.Logging;

namespace TestQit
{
    internal static class Program
    {
        private static ILogTransmitter _log;
        public static void Main()
        {
            Log.Logger = LogConfig.CreateLogger();
            LogManager.RegisterLogReceiver(new SerilogTransmitter());
            _log = LogManager.CreateLogger(nameof(Program));
            _log.Information("TestQit!");
            _log.Error(new Exception("Test exception"), "Error!!!!!");
            _log.Warning("Class: {ClassName}", nameof(Program));
        }
        
        private static LoggerConfiguration LogConfig { get; set; } = new LoggerConfiguration()
#if DEBUG
            /*.WriteTo.Debug()*/
            .MinimumLevel.Verbose()
#endif
            .WriteTo.Console()
            /*.WriteTo.File(formatter:new JsonFormatter(), path:Path.Combine(Environment.CurrentDirectory, "Logs", "log_.json"), rollingInterval: RollingInterval.Day)*/;
    }
}