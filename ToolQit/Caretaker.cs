using System;
using System.IO;
using Serilog;
using ToolQit.Containers;
using ToolQit.Serializers;

namespace ToolQit
{
    public static class Caretaker
    {
        static Caretaker()
        {
            AppDomain.CurrentDomain.ProcessExit += (_, _) =>
            {
                SaveSettings();
            };
            LoadSettings();
            ConfigPath = LibSettings["Paths"].GetString("AppSettings");
        }

        internal static readonly DataContainer LibSettings = SetupLibSettings(new DataContainer());
        
        /// <summary>
        /// Application settings (Settings.json)
        /// </summary>
        public static DataContainer Settings { get; private set; } = LibSettings["App.Settings"];


        // Config
        private static readonly string ConfigPath;
        private static readonly DataContainerJsonSerializer Serializer = new DataContainerJsonSerializer();

        public static void LoadSettings()
        {
            FileStream fsLoad;
            try
            {
                Log.Debug("Loading config from: {ConPath}", ConfigPath);
                fsLoad = File.Open(ConfigPath, FileMode.Open, FileAccess.Read);
            }
            catch (Exception)
            {
                Log.Warning("Could not load settings!");
                return;
            }

            if (Serializer.Deserialize(fsLoad, out object settings))
                Settings = (DataContainer)settings;

            fsLoad.Close();
        }

        public static void SaveSettings()
        {
            FileStream fsSave;
            try
            {
                Log.Debug("Saving config to: {ConPath}", ConfigPath);
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

        private static DataContainer SetupLibSettings(DataContainer container)
        {
            container["Paths"].Set("AppSettings", Path.Combine(Environment.CurrentDirectory, "Settings.json"));
            return container;
        }
    }
}