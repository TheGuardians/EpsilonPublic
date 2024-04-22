using EpsilonLib.Commands;

namespace CacheEditor.Components.TagExplorer.Commands
{
    [ExportCommand]
    class OpenTagCommand : CommandDefinition
    {
        public override string Name => "CacheEditor.OpenTag";

        public override string DisplayText => "Open Tag";
    }
}
