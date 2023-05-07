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
                Instances.Dispose();
            };
            LoadSettings();
            ConfigPath = LibSettings["Paths"].GetString("AppSettings");
        }

        internal static readonly DataContainer LibSettings = SetupLibSettings(new DataContainer());
        
        /// <summary>
        /// Application settings. Will be automatically saved to disk in Settings.json after application shutdown.
        /// </summary>
        public static DataContainer Settings { get; private set; } = LibSettings["App.Settings"];

        /// <summary>
        /// Stores instances. Auto disposes instances on process exit!
        /// </summary>
        public static InstanceContainer Instances { get; } = new InstanceContainer();


        // Config
        private static readonly string ConfigPath;
        private static readonly DataContainerJsonSerializer Serializer = new DataContainerJsonSerializer();

        public static bool ConfigExists() => File.Exists(ConfigPath);
        public static void LoadSettings()
        {
            FileStream fsLoad;
            try
            {
                Log.Debug("Loading config from: {ConPath}", ConfigPath);
                fsLoad = File.Open(ConfigPath, FileMode.Open, FileAccess.Read);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Could not load data!");
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