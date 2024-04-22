using EpsilonLib.Commands;

namespace CacheEditor.Components.TagTree.Commands
{
    [ExportCommand]
    public class ToggleFoldersViewCommand : CommandDefinition
    {
        public override string Name => "TagTree.ToggleFoldersView";

        public override string DisplayText => "Folders";
    }
}