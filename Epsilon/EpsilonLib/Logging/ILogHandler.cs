namespace EpsilonLib.Logging
{
    public interface ILogHandler
    {
        void Log(LogMessageType type, string message);
    }
}
