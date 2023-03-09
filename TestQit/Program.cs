using System;
using ToolQit;
using ToolQit.Containers;
using ToolQit.Serializers;
using ToolQit.Tools;

namespace TestQit
{
    internal static class Program
    {
        public static void Main()
        {
            Storage.Container.InitStorage();
            Console.WriteLine($"Welcome to {Storage.Container.GetString("Name")} Library!");
            var testJson = Serializer.Serialize(Storage.Container, new DataContainerJsonSerializer());
        }

        // Test values
        private static void InitStorage(this DataContainer container)
        {
            container.Set("Name", "ToolQit");
            container.Set("Type", "Lib");
            var env = container["Environment"];
            env.Set("cmd", Environment.CommandLine);
            env.Set("CurrentDir", Environment.CurrentDirectory);
            env.Set("Ticks", Environment.TickCount64);
            env.Set("User", Environment.UserName);
            var os = container["OS"];
            os.Set("Type", "Linux");
            os.Set("Is64", Environment.Is64BitOperatingSystem);
        }
    }
}