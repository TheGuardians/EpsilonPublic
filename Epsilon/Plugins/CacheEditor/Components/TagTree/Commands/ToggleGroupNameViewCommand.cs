using EpsilonLib.Commands;

namespace CacheEditor.Components.TagTree.Commands
{
    [ExportCommand]
    public class ToggleGroupNameViewCommand : CommandDefinition
    {
        public override string Name => "TagTree.ToggleGroupNameView";

        public override string DisplayText => "Full Name";
    }
}