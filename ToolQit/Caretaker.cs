using System;
using System.IO;
using ToolQit.Containers;
using ToolQit.Logging;
using ToolQit.Serializers;

namespace ToolQit
{
    public static class Caretaker
    {
        static Caretaker()
        {
            _log = LogManager.CreateLogger(nameof(Caretaker));
            AppDomain.CurrentDomain.ProcessExit += (_, _) =>
            {
                SaveSettings();
                Instances.Dispose();
            };
            LoadSettings();
            ConfigPath = LibSettings["Paths"].GetString("AppSettings");
        }

        private static ILogPipe _log;
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
        
        public static bool LoadSettings()
        {
            FileStream fsLoad;
            bool loaded = false;
            try
            {
                _log.Debug("Loading config from: {ConPath}", ConfigPath);
                fsLoad = File.Open(ConfigPath, FileMode.Open, FileAccess.Read);
            }
            catch (Exception ex)
            {
                _log.Fatal(ex, "Could not load data!");
                return loaded;
            }

            if (Serializer.Deserialize(fsLoad, out object settings))
            {
                Settings = (DataContainer)settings;
                loaded = true;
            }
            fsLoad.Close();
            return loaded;
        }

        public static bool SaveSettings()
        {
            FileStream fsSave;
            bool saved = false;
            try
            {
                _log.Debug("Saving config to: {ConPath}", ConfigPath);
                fsSave = File.Open(ConfigPath, FileMode.Create, FileAccess.Write);
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Cannot save settings!");
                return saved;
            }

            if (Serializer.Serialize(fsSave, Settings))
            {
                fsSave.Flush();
                saved = true;
            }
            fsSave.Close();
            return saved;
        }

        private static DataContainer SetupLibSettings(DataContainer container)
        {
            container["Paths"].Set("AppSettings", Path.Combine(Environment.CurrentDirectory, "Settings.json"));
            return container;
        }
    }
}