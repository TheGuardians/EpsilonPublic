using EpsilonLib.Commands;

namespace CacheEditor.Commands
{
    [ExportCommand]
    class ShowTagExplorerCommand : CommandDefinition
    {
        public override string Name => "CacheEditor.ShowTagExplorer";

        public override string DisplayText => "Tag Explorer";
    }
}
