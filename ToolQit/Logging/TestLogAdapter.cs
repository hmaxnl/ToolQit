using System;

namespace ToolQit.Logging
{
    public class TestLogAdapter : BaseLogAdapter
    {
        public TestLogAdapter()
        {
            
        }
        public override void OnReceive(LogEntry entry)
        {
            Console.WriteLine(entry.Template);
            base.OnReceive(entry);
        }
    }
}