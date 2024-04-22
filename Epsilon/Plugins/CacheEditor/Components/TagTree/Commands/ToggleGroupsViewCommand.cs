using EpsilonLib.Commands;

namespace CacheEditor.Components.TagTree.Commands
{
    [ExportCommand]
    public class ToggleGroupsViewCommand : CommandDefinition
    {
        public override string Name => "TagTree.ToggleGroupsView";

        public override string DisplayText => "Groups";
    }
}