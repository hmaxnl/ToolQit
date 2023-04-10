using System;
using System.IO;
using Serilog;
using Serilog.Core;
using Serilog.Formatting.Json;
using ToolQit.Containers;
using ToolQit.Serializers;

namespace ToolQit
{
    public static class Manager
    {
        static Manager()
        {
            AppDomain.CurrentDomain.ProcessExit += (_, _) =>
            {
                SaveSettings();
                Log?.Dispose();
            };
            Log = LogConfig.CreateLogger();
            LoadSettings();
        }

        #region Properties
        /// <summary>
        /// Configured logger used in the library and application.
        /// </summary>
        public static readonly Logger Log;

        /// <summary>
        /// Application settings (Settings.json)
        /// </summary>
        public static DataContainer Settings { get; private set; } = new DataContainer();
        
        
        // Config
        private static readonly string ConfigPath = Path.Combine(Environment.CurrentDirectory, "Settings.json");
        private static readonly DataContainerJsonSerializer Serializer = new DataContainerJsonSerializer();
        // Logging
        public static LoggerConfiguration LogConfig { get; set; } = new LoggerConfiguration()
        
#if DEBUG
            .WriteTo.Debug()
            .MinimumLevel.Verbose()
#endif
            .WriteTo.Console()
            .WriteTo.File(formatter:new JsonFormatter(), path:Path.Combine(Environment.CurrentDirectory, "Logs", "log_.json"), rollingInterval: RollingInterval.Day);
        #endregion

        #region Functions
        public static void LoadSettings()
        {
            FileStream fs;
            try
            {
                Log.Debug("Loading config from {ConPath}", ConfigPath);
                fs = File.Open(ConfigPath, FileMode.Open, FileAccess.Read);
            }
            catch (Exception)
            {
                Log.Warning("Could not load settings!");
                return;
            }

            if (Serializer.Deserialize(fs, out object settings))
                Settings = (DataContainer)settings;

            fs.Close();
        }

        public static void SaveSettings()
        {
            FileStream fsSave;
            try
            {
                fsSave = File.Open(ConfigPath, FileMode.Create, FileAccess.Write);
            }
            catch (Exception)
            {
                Log.Error("Cannot save settings!");
                return;
            }

            if (Serializer.Serialize(fsSave, Settings))
            {
                fsSave.Flush();
            }
            fsSave.Close();
        }
        #endregion
    }
}