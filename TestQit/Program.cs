using System;
using System.Text.Json;
using ToolQit.Collections;

namespace TestQit
{
    internal static class Program
    {
        public static void Main()
        {
            Console.WriteLine("Test!");
            DataCollection col = RandomCollection();
            DataCollection? copyCollection = JsonSerializer.Deserialize<DataCollection>(col.ToJson(), col.CollectionJsonOptions);
        }

        public static DataCollection RandomCollection()
        {
            DataCollection collection = new DataCollection();
            collection.Set("Test key", "Test value");
            collection.Set("AppName", "TestQit");
            collection.Set("Const", 9000);
            collection.Set("IsAvailable", true);
            collection.Set("Nice", 420.69);
            collection["App"].Set("Name", "TestQit");
            collection["App"].Set("Version", 1.2);
            collection["App"].Set("Release", false);
            collection["App.Constants"].Set("Index", 69);
            collection["App.Constants"].Set("CanSave", true);
            collection["App.Constants"].Set("SysPath", Environment.SystemDirectory);
            collection["App.Setting"].Set("CanLogin", true);
            collection["App.Setting"].Set("StartWithOS", false);
            collection["App.Setting"].Set("WelcomePrompt", "Welcome, back!");
            collection["Global"].Set("CurrentDir", Environment.CurrentDirectory);
            collection["Global"].Set("Cmd", Environment.CommandLine);
            collection["Global"].Set("MachineName", Environment.MachineName);
            collection["Global.OS.System"].Set("Process", Environment.ProcessPath ?? String.Empty);
            collection["Global.OS.System"].Set("UserName", Environment.UserName);
            collection["Global.OS.System"].Set("DomainName", Environment.UserDomainName);
            return collection;
        }
    }
}