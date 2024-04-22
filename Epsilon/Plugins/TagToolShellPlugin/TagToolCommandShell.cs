using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using TagTool.Cache;
using TagTool.Commands;
using TagTool.Commands.Common;
using TagTool.Commands.Tags;

namespace TagToolShellPlugin
{
    public class CommandShellOutputEventArgs
    {
        public string OutputLine { get; set; }
    }

    public sealed class TagToolCommandShell : ICommandShell, IDisposable
    {
        private CommandRunner _commandRunner;

        public event EventHandler<CommandShellOutputEventArgs> OutputReceived;
        public event EventHandler ContextChanged;

        public TagToolCommandShell(GameCache cache)
        {
            var contextStack = new CommandContextStack();
            contextStack.Push(TagCacheContextFactory.Create(contextStack, cache));
            _commandRunner = new CommandRunner(contextStack);
        }

        private List<string> _outputLineBuffer = new List<string>();

        public string ContextDisplayName => _commandRunner.ContextStack.Context?.Name;


        async Task ICommandShell.ExecuteAsync(string commandLine)
        {
            var oldContext = _commandRunner.ContextStack.Context;
            var oldOutHandle = Console.Out;
            Console.SetOut(new OutputWriter(Dispatcher.CurrentDispatcher, Console.Out.Encoding, Console_OnOutputLine));
            await Task.Run(() => _commandRunner.RunCommand(commandLine, false, true));
            Console.SetOut(oldOutHandle);

            if (_commandRunner.ContextStack.Context != oldContext)
                ContextChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            _commandRunner.ContextStack = null;
            _commandRunner = null;
        }

        private void Console_OnOutputLine(string line)
        {
            _outputLineBuffer.Add(line);
            OutputReceived?.Invoke(this, new CommandShellOutputEventArgs() { OutputLine = line });
        }

        class OutputWriter : TextWriter
        {
            private readonly Dispatcher _outputDispatcher;
            private readonly StringBuilder _buffer;
            private readonly Action<string> _writeLineDelegate;

            public OutputWriter(
                Dispatcher outputDispatcher,
                Encoding encoding,
                Action<string> writeLineDelegate)
            {
                _outputDispatcher = outputDispatcher;
                _buffer = new StringBuilder();
                _writeLineDelegate = writeLineDelegate;
                Encoding = encoding;
            }

            public override Encoding Encoding { get; }

            public override void Write(char[] buffer, int index, int count)
            {
                var line = new string(buffer, index, count - CoreNewLine.Length);
                _outputDispatcher.Invoke(() => _writeLineDelegate(line));
            }
        }
    }
}
