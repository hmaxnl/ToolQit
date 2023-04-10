using System;
using ToolQit;

namespace TestQit
{
    internal static class Program
    {
        public static void Main()
        {
            Console.WriteLine("TestQit!");
            var testVal = Manager.Settings.GetString("Test");
        }
    }
}