using EpsilonLib.Commands;

namespace CacheEditor.Components.TagTree.Commands
{
    [ExportCommand]
    public class CopyTagIndexCommand : CommandDefinition
    {
        public override string Name => "TagTree.CopyTagIndex";

        public override string DisplayText => "Copy Tag Index";
    }
}
