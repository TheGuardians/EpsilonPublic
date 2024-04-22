using EpsilonLib.Commands;

namespace CacheEditor.Components.TagExplorer.Commands
{
    [ExportCommand]
    class RenameTagCommand : CommandDefinition
    {
        public override string Name => "CacheEditor.RenameTag";

        public override string DisplayText => "Rename...";
    }
}
