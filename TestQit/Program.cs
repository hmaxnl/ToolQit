﻿using System;
using System.IO;
using System.Text;
using ToolQit;
using ToolQit.Containers;
using ToolQit.Serializers;

namespace TestQit
{
    internal static class Program
    {
        public static void Main()
        {
            Storage.Container.InitData();
            Console.WriteLine($"Welcome to {Storage.Container.GetString("Name")} Library!");
            MemoryStream jsonData = new MemoryStream();
            DataContainerJsonSerializer ser = new DataContainerJsonSerializer();
            ser.Serialize(jsonData, Storage.Container);
            string? jsonStr = Encoding.Default.GetString(jsonData.GetBuffer());
        }

        // Test values
        private static void InitData(this DataContainer container)
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