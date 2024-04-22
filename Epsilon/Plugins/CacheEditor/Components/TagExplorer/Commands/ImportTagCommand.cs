using EpsilonLib.Commands;

namespace CacheEditor.Components.TagExplorer.Commands
{
    [ExportCommand]
    class ImportTagCommand : CommandDefinition
    {
        public override string Name => "CacheEditor.ImportTag";

        public override string DisplayText => "Import...";
    }
}
