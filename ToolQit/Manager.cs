using System;
using System.IO;
using ToolQit.Containers;
using ToolQit.Serializers;

namespace ToolQit
{
    public static class Manager
    {
        static Manager()
        {
            AppDomain.CurrentDomain.ProcessExit += (sender, args) => SaveSettings();
            LoadSettings();
        }
        
        private static readonly string ConfigPath = Path.Combine(Environment.CurrentDirectory, "Settings.json");
        private static readonly DataContainerJsonSerializer Serializer = new DataContainerJsonSerializer();

        /// <summary>
        /// Application settings (Settings.json)
        /// </summary>
        public static DataContainer Settings { get; private set; } = new DataContainer();

        public static void LoadSettings()
        {
            FileStream fs;
            try
            {
                fs = File.Open(ConfigPath, FileMode.Open, FileAccess.Read);
            }
            catch (Exception)
            {
                //TODO: Log!
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
                //TODO: Log!
                return;
            }

            if (Serializer.Serialize(fsSave, Settings))
            {
                fsSave.Flush();
            }
            fsSave.Close();
        }
    }
}