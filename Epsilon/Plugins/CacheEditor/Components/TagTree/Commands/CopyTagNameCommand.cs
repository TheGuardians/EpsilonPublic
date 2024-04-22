using EpsilonLib.Commands;

namespace CacheEditor.Components.TagTree.Commands
{
    [ExportCommand]
    public class CopyTagNameCommand : CommandDefinition
    {
        public override string Name => "TagTree.CopyTagName";

        public override string DisplayText => "Copy Tag Name";
    }
}
