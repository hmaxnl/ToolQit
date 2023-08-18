namespace ToolQit.Logging
{
    public interface ILogReceiver
    {
        public long LogsReceived { get; }
        public void Receive(LogData data, ILogTransmitter sender);
    }
}