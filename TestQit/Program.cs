using ToolQit;
using ToolQit.Logging;

namespace TestQit
{
    internal static class Program
    {
        private static ILogPipe _log;
        public static void Main()
        {
            LogManager.RegisterLogOutput(new LogOutput());
            _log = LogManager.CreateLogger(nameof(Program));
            _log.Information("TestQit!");
            /*var testVal = Caretaker.Settings.GetString("Test");
            _log.Information("Value from settings: {0}", testVal);*/
        }
        
        /*private static LoggerConfiguration LogConfig { get; set; } = new LoggerConfiguration()
        
#if DEBUG
            .WriteTo.Debug()
            .MinimumLevel.Verbose()
#endif
            .WriteTo.Console()
            .WriteTo.File(formatter:new JsonFormatter(), path:Path.Combine(Environment.CurrentDirectory, "Logs", "log_.json"), rollingInterval: RollingInterval.Day);*/
    }
}