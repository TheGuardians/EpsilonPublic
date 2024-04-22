using EpsilonLib.Commands;

namespace CacheEditor.Components.TagExplorer.Commands
{
    [ExportCommand]
    class ExtractTagCommand : CommandDefinition
    {
        public override string Name => "CacheEditor.ExtractTag";

        public override string DisplayText => "Extract...";
    }
}
