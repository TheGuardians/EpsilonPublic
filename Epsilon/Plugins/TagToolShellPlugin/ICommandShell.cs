using System;
using System.Threading.Tasks;

namespace TagToolShellPlugin
{
    public interface ICommandShell : IDisposable
    {
        string ContextDisplayName { get; }
        event EventHandler ContextChanged;

        event EventHandler<CommandShellOutputEventArgs> OutputReceived;

        Task ExecuteAsync(string commandLine);
    }
}
