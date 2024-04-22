using EpsilonLib.Logging;
using System.Diagnostics;
using System.IO;

namespace Epsilon.Logging
{
    class DefaultLogger : ILogHandler
    {
        const string FileName = "epsilon.log";

        private StreamWriter _writer;

        public DefaultLogger()
        {
            _writer = File.AppendText(FileName);
        }

        public void Log(LogMessageType type, string message)
        {
            var line = $"[{ type.ToString().ToUpper()}]: { message}";
            Debug.WriteLine(line);
            _writer.WriteLine(line);
            _writer.Flush();
        }
    }
}
