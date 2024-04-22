using EpsilonLib.Commands;

namespace TagToolShellPlugin.Commands
{
    [ExportCommand]
    public class ShowShellWindowCommand : CommandDefinition
    {
        public override string Name => "CacheEditor.ShowShell";

        public override string DisplayText => "Shell";
    }
}
