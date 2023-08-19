using System;

namespace ToolQit.Logging
{
    public abstract class BaseLogAdapter : IDisposable
    {
        public long LogCount { get; private set; }
        internal event Action<BaseLogAdapter>? DisposeAdapter;
        public virtual void OnReceive(LogEntry entry)
        {
            LogCount++;
        }
        public virtual void Dispose()
        {
            
        }
        
        ~BaseLogAdapter()
        {
            DisposeAdapter?.Invoke(this);
            Dispose();
        }
    }
}