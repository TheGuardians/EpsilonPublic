using EpsilonLib.Commands;

namespace CacheEditor.Components.TagTree.Commands
{
    [ExportCommand]
    public class ToggleGroupTagNameViewCommand : CommandDefinition
    {
        public override string Name => "TagTree.ToggleGroupTagNameView";

        public override string DisplayText => "Abbreviated";
    }
}