using EpsilonLib.Commands;

namespace CacheEditor.Components.TagExplorer.Commands
{
    [ExportCommand]
    class DuplicateTagCommand : CommandDefinition
    {
        public override string Name => "CacheEditor.DuplicateTag";

        public override string DisplayText => "Duplicate...";
    }
}
