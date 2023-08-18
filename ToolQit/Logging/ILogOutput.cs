namespace ToolQit.Logging
{
    public interface ILogOutput
    {
        public long LogsHandled { get; }
        public void Receive(LogData data, ILogPipe sender);
    }
}