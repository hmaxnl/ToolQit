using System;
using ToolQit;

namespace TestQit
{
    internal static class Program
    {
        public static void Main()
        {
            Console.WriteLine($"Library name: {Global.Data.GetString("Name")}");
        }
    }
}