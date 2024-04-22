using EpsilonLib.Commands;

namespace CacheEditor.Commands
{
    [ExportCommand]
    class ShowCommandLogCommand : CommandDefinition
    {
        public override string Name => "CacheEditor.ShowCommandLog";

        public override string DisplayText => "Command Log";
    }
}
