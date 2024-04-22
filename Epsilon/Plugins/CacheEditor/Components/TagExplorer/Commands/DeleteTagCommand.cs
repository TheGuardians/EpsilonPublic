using EpsilonLib.Commands;

namespace CacheEditor.Components.TagExplorer.Commands
{
    [ExportCommand]
    class DeleteTagCommand : CommandDefinition
    {
        public override string Name => "CacheEditor.DeleteTag";

        public override string DisplayText => "Delete";
    }
}
