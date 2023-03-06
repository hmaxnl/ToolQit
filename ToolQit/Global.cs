using System;
using System.IO;
using ToolQit.Containers;

namespace ToolQit
{
    /// <summary>
    /// A class that can be used globally to manage data. Use with care!
    /// </summary>
    public static class Global
    {
        static Global()
        {
            Data._setupData();
            Load();
        }
        
        public static readonly DataContainer Data = new DataContainer();

        
        // Internal library stuff

        internal static readonly DataContainer LibData = new DataContainer();
        private static void _setupData(this DataContainer collection)
        {
            // Global data
            Data.Set("Name", "ToolQit");
            Data.Set("LogPath", Path.Combine(Environment.CurrentDirectory, "log"));
            Data["OS"].Set("CommandLine", Environment.CommandLine);
            Data.Set("UserInteractive", Environment.UserInteractive);
            Data.Set("pid", Environment.ProcessId);
            
            // Lib data
            LibData["Defaults.Paths"].Set("MainStorage", Path.Combine(Environment.CurrentDirectory, "app_data.json"));
        }

        internal static void Save()
        {
        }

        internal static void Load()
        {
        }

        internal static void SetupLogging()
        {
        }
    }
}