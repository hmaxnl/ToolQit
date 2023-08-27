using System;
using Serilog;
using ToolQit;
using ToolQit.Logging;
using ToolQit.Logging.Serilog;

namespace TestQit
{
    internal static class Program
    {
        private static ILog _log;
        public static void Main()
        {
            Log.Logger = LogConfig.CreateLogger();
            LogManager.RegisterAdapter(new SerilogAdapter(Log.Logger));
            _log = LogManager.CreateLogger(typeof(Program));
            _log.Information("TestQit!");
            _log.Error(new Exception("Test exception"), "Error!!!!!");
            _log.Warning("Class: {ClassName}", nameof(Program));
        }
        
        private static readonly LoggerConfiguration LogConfig = new LoggerConfiguration()
#if DEBUG
            /*.WriteTo.Debug()*/
            .MinimumLevel.Verbose()
#endif
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} - {Sender} | {Level:u3}] {Message:lj}{NewLine}{Exception}")
            /*.WriteTo.File(formatter:new JsonFormatter(), path:Path.Combine(Environment.CurrentDirectory, "Logs", "log_.json"), rollingInterval: RollingInterval.Day)*/;
    }
}