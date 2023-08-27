using System;

namespace ToolQit.Logging
{
    public abstract class BaseLogAdapter : IDisposable
    {
        public BaseLogAdapter()
        {
            Id = Guid.NewGuid().ToString();
            LogManager.EmitLog += OnReceive;
        }
        public string Id { get; }
        public long LogCount { get; private set; }
        public virtual void OnReceive(LogEntry entry)
        {
            LogCount++;
        }
        public virtual void Dispose()
        {
            
        }
        public override bool Equals(object obj)
        {
            if (obj is BaseLogAdapter objAdapter)
                return objAdapter.Id == Id;
            return false;
        }

        public override int GetHashCode() => Id.GetHashCode();

        private Action<LogEntry>? _emitToReceive;

        internal void RegisterEvent(ref Action<LogEntry>? emitEvent)
        {
            _emitToReceive = emitEvent;
            _emitToReceive += OnReceive;
        }

        ~BaseLogAdapter()
        {
            _emitToReceive -= OnReceive;
            Dispose();
        }
    }
}