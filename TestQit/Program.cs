using System;
using System.IO;
using ToolQit;
using ToolQit.Serializers;

namespace TestQit
{
    internal static class Program
    {
        public static void Main()
        {
            Console.WriteLine($"Library name: {Global.Data.GetString("Name")}");

            MemoryStream jsonStream = new MemoryStream();
            DataContainerJsonSerializer ser = new DataContainerJsonSerializer();
            if (!ser.Serialize(jsonStream, Global.Data)) return;
            var json = System.Text.Encoding.UTF8.GetString(jsonStream.ToArray());
        }
    }
}