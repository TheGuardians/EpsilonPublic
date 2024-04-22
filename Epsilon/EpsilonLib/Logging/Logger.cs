using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Windows.Navigation;
using TagTool.Cache;

namespace EpsilonLib.Logging
{
    public static class Logger
    {
        private volatile static IList<ILogHandler> _loggers = new List<ILogHandler>();

        public static event EventHandler CommandLogChanged;
        private static List<CommandEvent> CommandLog = new List<CommandEvent>();
        public static CachedTag ActiveTag;
        public static void Trace(string message) => Log(LogMessageType.Trace, message);
        public static void Info(string message) => Log(LogMessageType.Info, message);
        public static void Warn(string message) => Log(LogMessageType.Warning, message);
        public static void Error(string message) => Log(LogMessageType.Error, message);
        public static void LogCommand(string tag, string field, CommandEvent.CommandType type, string command)
        {
            var newLog = new CommandEvent
            {
                Tag = tag,
                Field = field,
                Type = type,
                CommandText = command
            };

            //see if a setfield exists for the current field in the current tag already
            int match = CommandLog.FindIndex(l => l.Type == CommandEvent.CommandType.setfield &&
            l.Tag == tag && l.Field == field);

            if(match != -1)
            {
                CommandLog[match] = newLog;
            }
            else
                CommandLog.Add(newLog);
            CommandLogChanged?.Invoke(null, EventArgs.Empty);
        }
        public static string GetCommandLogText()
        {
            List<string> commandtextlist = CommandLog.Select(c => c.CommandText).ToList();
            return string.Join("\n", commandtextlist);
        }
        public static void ClearCommandLog()
        {
            CommandLog.Clear();
            CommandLogChanged?.Invoke(null, EventArgs.Empty);
        }
        public static void RegisterLogger(ILogHandler logger)
        {
            lock (_loggers)
            {
                if (_loggers.Contains(logger))
                    return;

                _loggers.Add(logger);
            }
        }

        private static void Log(LogMessageType type, string message)
        {
            lock(_loggers) 
            {
                foreach (var logger in _loggers)
                    logger.Log(type, message);
            }
        }

        public class CommandEvent
        {
            public string Tag;
            public string Field;
            public CommandType Type;
            public string CommandText;

            public enum CommandType
            {
                none,
                setfield
            }
        }
    }
}
