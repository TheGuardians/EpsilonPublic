using EpsilonLib.Commands;

namespace CacheEditor.Commands
{
    [ExportCommand]
    class ShowDependencyExplorerCommand : CommandDefinition
    {
        public override string Name => "CacheEditor.ShowDependencyExplorer";

        public override string DisplayText => "Dependency Explorer";
    }
}
